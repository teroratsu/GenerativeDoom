using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeImp.DoomBuilder;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Config;
using System.Threading;
using System.Runtime.Serialization.Json;
using CodeImp.DoomBuilder.Editing;
using System.IO;

namespace GenerativeDoom
{
    public partial class GDForm : Form
    {
        private static Random rng = new Random();
        public const int TYPE_PLAYER_START = 1;

        private IList<DrawnVertex> points;

        public GDForm()
        {
            InitializeComponent();
            points = new List<DrawnVertex>();
        }

        // We're going to use this to show the form
        public void ShowWindow(Form owner)
        {
            // Position this window in the left-top corner of owner
            this.Location = new Point(owner.Location.X + 20, owner.Location.Y + 90);

            // Show it
            base.Show(owner);
        }

        // Form is closing event
        private void GDForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When the user is closing the window we want to cancel this, because it
            // would also unload (dispose) the form. We only want to hide the window
            // so that it can be re-used next time when this editing mode is activated.
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Just cancel the editing mode. This will automatically call
                // OnCancel() which will switch to the previous mode and in turn
                // calls OnDisengage() which hides this window.
                General.Editing.CancelMode();
                e.Cancel = true;
            }
        }

        private void GDForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            General.Editing.CancelMode();
        }

        private List<Sector> newSector(IList<DrawnVertex> points, int lumi, int ceil, int floor)
        {
            Console.Write("New sector: ");
            List<DrawnVertex> pSector = new List<DrawnVertex>();

            DrawnVertex v;
            v = new DrawnVertex();
            foreach (DrawnVertex p in points)
            {
                Console.Write(" p:"+p.pos);
                v.pos = p.pos;
                v.stitch = true;
                v.stitchline = true;
                pSector.Add(v);
            }

            Console.Write("\n");

            v.pos = points[0].pos;
            v.stitch = true;
            v.stitchline = true;

            pSector.Add(v);

            Tools.DrawLines(pSector);

            // Snap to map format accuracy
            General.Map.Map.SnapAllToAccuracy();

            // Clear selection
            General.Map.Map.ClearAllSelected();

            // Update cached values
            General.Map.Map.Update();

            // Edit new sectors?
            List<Sector> newsectors = General.Map.Map.GetMarkedSectors(true);

            foreach (Sector s in newsectors)
            {
                s.CeilHeight = ceil;
                s.FloorHeight = floor;
                s.Brightness = lumi;
            }

            // Update the used textures
            General.Map.Data.UpdateUsedTextures();

            // Map is changed
            General.Map.IsChanged = true;

            General.Interface.RedrawDisplay();

            //Ajoute, on enleve la marque sur les nouveaux secteurs
            General.Map.Map.ClearMarkedSectors(false);

            return newsectors;
        }

        private List<Sector> newSector(DrawnVertex top_left, float width, float height, int lumi, int ceil, int floor)
        {
            points.Clear();
            DrawnVertex v;
            v = new DrawnVertex();
            v.pos = top_left.pos;
            points.Add(v);

            v.pos.y += height;
            points.Add(v);

            v.pos.x += width;
            
            points.Add(v);

            v.pos.y -= height;
            points.Add(v);

            List<Sector> newsectors = newSector(points,lumi,ceil,floor);
            points.Clear();

            return newsectors;
        }

        private Thing addThing(Vector2D pos, String category, float proba = 0.5f)
        {
            Thing t = addThing(pos);
            if (t != null)
            {

                IList<ThingCategory> cats = General.Map.Data.ThingCategories;
                Random r = new Random();

                bool found = false;
                foreach (ThingTypeInfo ti in General.Map.Data.ThingTypes)
                {
                    if (ti.Category.Name == category)
                    {
                        t.Type = ti.Index;
                        Console.WriteLine("Add thing cat " + category + " for thing at pos " + pos);
                        found = true;
                        if (r.NextDouble() > proba)                     
                            break;
                    }

                }
                if (!found)
                {
                    Console.WriteLine("###### Could not find category " + category + " for thing at pos " + pos);
                }else
                    t.Rotate(0);
            }else
            {
                Console.WriteLine("###### Could not add thing for cat " + category + " at pos " + pos);
            }

            return t;
        }

        private Thing addThing(Vector2D pos)
        {
            if (pos.x < General.Map.Config.LeftBoundary || pos.x > General.Map.Config.RightBoundary ||
                pos.y > General.Map.Config.TopBoundary || pos.y < General.Map.Config.BottomBoundary)
            {
                Console.WriteLine( "Error Generaetive Doom: Failed to insert thing: outside of map boundaries.");
                return null;
            }

            // Create thing
            Thing t = General.Map.Map.CreateThing();
            if (t != null)
            {
                General.Settings.ApplyDefaultThingSettings(t);

                t.Move(pos);

                t.UpdateConfiguration();

                // Update things filter so that it includes this thing
                General.Map.ThingsFilter.Update();

                // Snap to map format accuracy
                t.SnapToAccuracy();
            }

            return t;
        }

        private void correctMissingTex()
        {

            String defaulttexture = "-";
            if (General.Map.Data.TextureNames.Count > 1)
                defaulttexture = General.Map.Data.TextureNames[1];

            // Go for all the sidedefs
            foreach (Sidedef sd in General.Map.Map.Sidedefs)
            {
                // Check upper texture. Also make sure not to return a false
                // positive if the sector on the other side has the ceiling
                // set to be sky
                if (sd.HighRequired() && sd.HighTexture[0] == '-')
                {
                    if (sd.Other != null && sd.Other.Sector.CeilTexture != General.Map.Config.SkyFlatName)
                    {
                        sd.SetTextureHigh(General.Settings.DefaultCeilingTexture);
                    }
                }

                // Check middle texture
                if (sd.MiddleRequired() && sd.MiddleTexture[0] == '-')
                {
                    sd.SetTextureMid(General.Settings.DefaultTexture);
                }

                // Check lower texture. Also make sure not to return a false
                // positive if the sector on the other side has the floor
                // set to be sky
                if (sd.LowRequired() && sd.LowTexture[0] == '-')
                {
                    if (sd.Other != null && sd.Other.Sector.FloorTexture != General.Map.Config.SkyFlatName)
                    {
                        sd.SetTextureLow(General.Settings.DefaultFloorTexture);
                    }
                }

            }
        }

        private bool checkIntersect(Line2D measureline)
        {
            bool inter = false;
            foreach (Linedef ld2 in General.Map.Map.Linedefs)
            {
                // Intersecting?
                // We only keep the unit length from the start of the line and
                // do the real splitting later, when all intersections are known
                float u;
                if (ld2.Line.GetIntersection(measureline, out u))
                {
                    if (!float.IsNaN(u) && (u > 0.0f) && (u < 1.0f))
                    {
                        inter = true;
                        break;
                    }
                       
                }
            }

            Console.WriteLine("Chevk inter " + measureline + " is " + inter);

            return inter;
        }

        /*
        private bool checkIntersect(Line2D measureline, out float closestIntersect)
        {
            // Check if any other lines intersect this line
            List<float> intersections = new List<float>();
            foreach (Linedef ld2 in General.Map.Map.Linedefs)
            {
                // Intersecting?
                // We only keep the unit length from the start of the line and
                // do the real splitting later, when all intersections are known
                float u;
                if (ld2.Line.GetIntersection(measureline, out u))
                {
                   if (!float.IsNaN(u) && (u > 0.0f) && (u < 1.0f))
                        intersections.Add(u);
                }
            }

            if(intersections.Count() > 0)
            {
                // Sort the intersections
                intersections.Sort();

                closestIntersect = intersections.First<float>();

                return true;
            }

            closestIntersect = 0.0f;
            return false;

            
        }
         * */
    

        private void showCategories()
        {
            lbCategories.Items.Clear();
            IList<ThingCategory> cats = General.Map.Data.ThingCategories;
            foreach(ThingCategory cat in cats)
            {
                if (!lbCategories.Items.Contains(cat.Name))
                    lbCategories.Items.Add(cat.Name);
            }
            
        }
        /*
        private void makeOnePath( bool playerStart,int rNumber = 5, float dWidth = 32f, float dSize = 16f, float dHeight = 64f)
        {
            Random r = new Random();

            DrawnVertex v = new DrawnVertex();
            DrawnVertex v_c = new DrawnVertex();
            DrawnVertex v_c_door = new DrawnVertex();

            IList<Sector> sectors = new List<Sector>(); // list of created sectors

            float doorWidth = dWidth;
            float doorHeight = dHeight;
            float doorsize = dSize;

            float pwidth = 0.0f;
            float pheight = 0.0f;

            int lumi = 200;
            int ceil = (int)(r.NextDouble() * 128 + 128);
            int floor = (int)(r.NextDouble() * 128 + 128);
            int pfloor = floor; 
            int pceil = ceil;
            Vector2D pv = new Vector2D();
            int pdir = 0;
            for (int index = 0; index < rNumber ; index++)
            {
                pv = v.pos;

                //Taille du prochain secteur
                float width = (float)(r.Next() % 10) * 64.0f + 128.0f;
                float height = (float)(r.Next() % 10) * 64.0f + 128.0f;

                //On checke ou on peut le poser
                Line2D l1 = new Line2D();
                bool[] dirOk = new bool[4];

                //A droite
                l1.v2 = l1.v1 = pv;
                l1.v1.x += pwidth;
                l1.v2.x += width + 2048;
                bool droite = !checkIntersect(l1);
                l1.v1.y = l1.v2.y = l1.v1.y + height;
                droite = droite && !checkIntersect(l1);
                dirOk[0] = droite;
                
                //A gauche
                l1.v2 = l1.v1 = pv;
                l1.v2.x -= width + 2048;
                bool gauche = !checkIntersect(l1);
                l1.v1.y = l1.v2.y = l1.v1.y + height;
                gauche = gauche && !checkIntersect(l1);
                dirOk[1] = gauche;

                //En haut
                l1.v2 = l1.v1 = pv;
                l1.v1.y = l1.v2.y = l1.v1.y + pheight;
                l1.v2.y += height + 2048;
                bool haut = !checkIntersect(l1);
                l1.v1.x = l1.v2.x = l1.v1.x + width;
                haut = haut && !checkIntersect(l1);
                dirOk[2] = haut;

                //En bas
                l1.v2 = l1.v1 = pv;
                l1.v2.y -= height + 2048;
                bool bas = !checkIntersect(l1);
                l1.v1.x = l1.v2.x = l1.v1.x + width;
                bas = bas && !checkIntersect(l1);
                dirOk[3] = bas;

                bool oneDirOk = haut || bas || gauche || droite;

                int nextDir = pdir;
                if (!oneDirOk)
                    Console.WriteLine("No dir available, on va croiser !!!");
                else
                {
                    int nbTry = 0;
                    while ((!dirOk[nextDir] || pdir == nextDir) && nbTry++ < 100)
                        nextDir = r.Next() % 4;
                }

                bool isHorizontalDoor = false;
                v_c_door = v_c = v;

                switch (nextDir)
                {
                    case 0: //droite
                        v.pos.x += pwidth;
                        v_c.pos.x += pwidth;
                        v_c_door.pos.x = v_c.pos.x + doorWidth / 4f + doorsize/2f;
                        v.pos.x += doorWidth;
                        break;
                    case 1: //gauche
                        v.pos.x -= width;
                        v_c.pos.x -= doorWidth;
                        v_c_door.pos.x = v_c.pos.x + doorWidth / 4f + doorsize / 2f;
                        v.pos.x -= doorWidth;
                        break;
                    case 2: //haut
                        v.pos.y += pheight;
                        v_c.pos.y += pheight;
                        v_c_door.pos.y = v_c.pos.y + doorWidth / 4f + doorsize / 2f;
                        v.pos.y += doorWidth;
                        isHorizontalDoor = true;
                        break;
                    case 3: //bas
                        v.pos.y -= height;
                        v_c.pos.y -= doorWidth;
                        v_c_door.pos.y = v_c.pos.y + doorWidth / 4f + doorsize / 2f;
                        v.pos.y -= doorWidth;
                        isHorizontalDoor = true;
                        break;
                }

                lumi = Math.Min(256, Math.Max(0, lumi + (r.NextDouble() > 0.5 ? 1 : -1) * 16));
                
                if (ceil - floor < 100)
                {
                    floor = pfloor;
                    ceil = floor + 180 + (r.NextDouble() > 0.5 ? 1 : -1) * 16;
                }
                if(index > 0) // not the first room
                {//then build a door
                    if (isHorizontalDoor)
                    {
                        newSector(v_c, doorHeight,
                            doorWidth,
                            lumi,
                            floor + (int)doorHeight,
                            floor);
                        newSector(v_c_door, doorHeight,
                            doorsize,
                            lumi,
                            floor + (int)doorHeight,
                            floor);
                    }
                    else
                    {
                        newSector(v_c, doorWidth,
                            doorHeight,
                            lumi,
                            floor + (int)doorHeight,
                            floor);
                        newSector(v_c_door, doorsize,
                            doorHeight,
                            lumi,
                            floor + (int)doorHeight,
                            floor);
                    }
                    sectors = General.Map.Map.Sectors as IList<Sector>;
                    Sector s = sectors[sectors.Count - 1];
                    s.SetCeilTexture("SLIME15");
                    s.SetFloorTexture("SLIME14");
                    s.CeilHeight -= (int) doorHeight;
                    int i = 0;
                    if (isHorizontalDoor)
                    {
                        foreach (Sidedef side in s.Sidedefs)
                        {
                            if (i % 2 != 0)
                            {
                                side.Line.FlipVertices();
                                side.SetTextureHigh("BIGDOOR4");
                                side.Line.Action = 1;
                            }
                            else
                            {
                                side.Line.SetFlag(General.Map.Config.LowerUnpeggedFlag, true);
                            }
                            ++i;
                        }
                        newSector(v_c, doorHeight,
                           doorWidth,
                           lumi,
                           floor + (int)doorHeight,
                           floor);
                    }
                    else
                    {
                        foreach (Sidedef side in s.Sidedefs)
                        {
                            if (i % 2 == 0)
                            {
                                side.Line.FlipVertices();
                                side.SetTextureHigh("BIGDOOR4");
                                side.Line.Action = 1;
                            }
                            else
                            {
                                side.Line.SetFlag(General.Map.Config.LowerUnpeggedFlag, true);
                            }
                            ++i;
                        }
                        // Update the used textures
                        General.Map.Data.UpdateUsedTextures();

                        newSector(v_c, doorWidth,
                           doorHeight,
                           lumi,
                           floor + (int)doorHeight,
                           floor);
                    }
                    
                }
                //((ClassicMode)General.Editing.Mode).CenterOnArea();
                newSector(v, width,
                    height,
                    lumi,
                    ceil,
                    floor);

                //showSectors();

                //Faire une porte avec une nouvelle linedef sur son secteur
                if (index == 0 && playerStart)
                {
                    Thing t = addThing(new Vector2D(v.pos.x + width / 2, v.pos.y + height / 2));
                    t.Type = TYPE_PLAYER_START;
                    t.Rotate(0);
                }
                else if (index == 1)
                {
                    addThing(new Vector2D(v.pos.x + width / 2, v.pos.y + height / 2), "weapons");
                }
                else if (index % 3 == 0)
                {
                    while (r.NextDouble() > 0.3f)
                        addThing(new Vector2D(v.pos.x + width / 2 + ((float)r.NextDouble() * (width / 2)) - width / 4,
                            v.pos.y + height / 2 + ((float)r.NextDouble() * (height / 2)) - height / 4), "monsters");
                }
                if (index % 5 == 0)
                {
                    do
                    {

                        addThing(new Vector2D(v.pos.x + width / 2 + ((float)r.NextDouble() * (width / 2)) - width / 4,
                            v.pos.y + height / 2 + ((float)r.NextDouble() * (height / 2)) - height / 4), "ammunition");
                    } while (r.NextDouble() > 0.3f);
                }
                if (index % 20 == 0)
                {
                    do
                    {
                        addThing(new Vector2D(v.pos.x + width / 2 + ((float)r.NextDouble() * (width / 2)) - width / 4,
                            v.pos.y + height / 2 + ((float)r.NextDouble() * (height / 2)) - height / 4), "health");
                    } while (r.NextDouble() > 0.5f);
                    addThing(new Vector2D(v.pos.x + width / 2, v.pos.y + height / 2), "weapons", 0.3f);
                }

                while (r.NextDouble() > 0.5f)
                    addThing(new Vector2D(v.pos.x + width / 2 + ((float)r.NextDouble() * (width / 2)) - width / 4,
                        v.pos.y + height / 2 + ((float)r.NextDouble() * (height / 2)) - height / 4), "decoration", (float)r.NextDouble());
                
                pwidth = width;
                pheight = height;
                pceil = ceil;
                pfloor = floor;
                pdir = nextDir;
                try { Thread.Sleep(0); }
                catch (ThreadInterruptedException) { Console.WriteLine(">>>> thread de generation interrompu at sector " + index); break; }
            }
        }
        */

        private List<Room> deserializePrefabsInfo()
        {
            List<Room> rooms = new List<Room>();
            bool readAllFiles = false;
            int iterationCount = 0;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Room));

            while (!readAllFiles)
            {
                string path = "prefabs/room" + iterationCount + ".json";

                if (!File.Exists(path))
                {
                    readAllFiles = true;
                }
                else
                {
                    MemoryStream ms = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(File.ReadAllText(path)));
                    rooms.Add((Room)ser.ReadObject(ms));
                    ++iterationCount;
                    ms.Close();
                }
            }
            
            return rooms;
        }
        
        private void serializePrefabInfo()
        {
            Room newRoom = new Room(prefabObjectPath.Text, (RoomType)Enum.Parse(typeof(RoomType), RoomType_t.SelectedItem.ToString(), true));

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Room));
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, newRoom);

            //save file
            File.WriteAllBytes("Prefabs/room" + roomID.Value.ToString() + ".json", ms.ToArray());
        }

        private void showSectors()
        {
            ICollection<Sector> sectors = General.Map.Map.Sectors;
            foreach (Sector s in sectors)
            {
                Console.WriteLine(s.ToString());
            }
            Console.WriteLine("count : " + sectors.Count);
        }

        private void btnDoMagic_Click(object sender, EventArgs e)
        {
            correctMissingTex();
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            showCategories();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            generateLD(
                Convert.ToInt32(roomNb.Text),
                Convert.ToInt32(doorWidth.Text),
                Convert.ToInt32(doorSize.Text),
                Convert.ToInt32(doorHeight.Text),
                10,
                2,
                1
                );

            correctMissingTex();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //TODO pass roomNumber, amount of keys (6 max), amount of boss.
        private void generateLD(int roomNb, int dWidth, int dSize, int dHeight, int roomAmount = 10, int lockedRoomAmount = 2, int bossRoomAmount = 1)
        {
            //clear map
            General.Map.map.Dispose();
            General.Map.map = new MapSet();

            GeneratorParameters parameters = new GeneratorParameters();
            List<Room> allRooms = deserializePrefabsInfo();
            List<Room> rooms = new List<Room>();
            try
            {
                if (lockedRoomAmount > 6) throw new LDGenerationException("max 6 key allowed");
                if (roomAmount < lockedRoomAmount*2 + bossRoomAmount) 
                    throw new LDGenerationException("The roomAmount should be higher than the amount of 2x the amount of locked rooms + the amount of boss rooms");
                if (roomAmount < 2) throw new LDGenerationException("min 2 rooms");

                List<Room> BossRooms = allRooms.Where(room => room.type == RoomType.Boss).ToList();
                List<Room> lockedRooms = allRooms.Where( room => room.type == RoomType.Locked).ToList();
                List<Room> keyRooms = allRooms.Where(room => room.type == RoomType.Key).ToList();
                List<Room> normalRooms = allRooms.Where(room => room.type == RoomType.Basic).ToList();
                List<Room> spawnRooms = allRooms.Where(room => room.type == RoomType.Spawn).ToList();
                List<Room> endRooms = allRooms.Where(room => room.type == RoomType.End).ToList();

                if (lockedRooms.Count == 0 && lockedRoomAmount > 0) 
                    throw new LDGenerationException("no locked room found, can't generate LD with the specified parameters");
                if(spawnRooms.Count == 0 || endRooms.Count == 0)
                    throw new LDGenerationException("Please create at least one spawn and end room to be able to generate a level");

                //make a list of rooms using specified parameters
                List<Room> selectedRooms = new List<Room>();
                int locked_t = lockedRoomAmount;
                int boss_t = bossRoomAmount;
                int i = roomAmount - 2; // minus spawn and last room
                while (i > 1)
                {
                    // first we add the locked rooms
                    if(locked_t > 0)
                    {
                        --locked_t;
                        selectedRooms.Add(new Room(lockedRooms[rng.Next(lockedRooms.Count)]));
                        --i;
                    }
                    else
                    {
                        //then the boss rooms
                        if(boss_t > 0)
                        {
                            --boss_t;
                            selectedRooms.Add(new Room(BossRooms[rng.Next(BossRooms.Count)]));
                            --i;
                        }
                        else
                        {
                            // and finally the normal rooms
                            selectedRooms.Add(new Room(normalRooms[rng.Next(normalRooms.Count)]));
                            --i;
                        }
                    }
                }
                // shuffle
                int n = selectedRooms.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    Room value = selectedRooms[k];
                    selectedRooms[k] = selectedRooms[n];
                    selectedRooms[n] = value;
                }  
                // add key rooms
                locked_t = lockedRoomAmount;
                for(i = 0; i< selectedRooms.Count; ++i)
                {
                    if(selectedRooms[i].type == RoomType.Locked && locked_t > 0)
                    {
                        --locked_t;
                        //check if there is enough key to unlock the locked doors, if not, insert a new one
                        if (selectedRooms.GetRange(0, i).Where(room => room.type == RoomType.Key).ToList().Count < lockedRoomAmount - locked_t)
                        {
                            selectedRooms.Insert(rng.Next(i), new Room(keyRooms[rng.Next(keyRooms.Count)]));
                            ++i; //forward one step because we inserted a new item
                        }
                    }
                }
                // add spawn room
                selectedRooms.Insert(0, new Room(spawnRooms[rng.Next(spawnRooms.Count)]));
                // add end room
                selectedRooms.Add(new Room(endRooms[rng.Next(endRooms.Count)]));

                rooms = selectedRooms;
                parameters.doorwaySize = dWidth;
                parameters.doorSize = dSize;
                parameters.doorHeight = dHeight;
            }
            catch(LDGenerationException ex)
            {
                Console.WriteLine(ex.Source + " : " + ex.Message);
            }
            
            if(rooms != null)
            {
                // Create undo
                General.Map.UndoRedo.CreateUndo("Generate LD");
                   
                //create rooms recursively 
                createRooms(rooms, parameters);
                createDoors(ref rooms, parameters);

                //fit in screen
                ((ClassicMode)General.Editing.Mode).CenterInScreen();
            }
            
        }

        
        private void createRooms(List<Room> rooms, GeneratorParameters parameters)
        {
            Room p = null;

            //attribute Room order based on a simple algorithm
            foreach(Room c in rooms)
            {
                if (p == null)
                {
                    c.dir = RoomDir.STATIC;
                } 
                else
                {
                    double rnd = rng.NextDouble();
                    if (p.dir == RoomDir.Down)
                    {
                        c.dir = ( rnd > .5) ? RoomDir.Left : RoomDir.Right;
                    }
                    else
                    {
                        RoomDir pDir = p.dir;
                        c.dir = ( rnd > .3) ? RoomDir.Down : pDir;
                    }

                    if (p.dir == RoomDir.STATIC)
                        c.dir = RoomDir.Down;
                }
                p = new Room(c);
            }

            Vector2D position = new Vector2D(0, 0);

            //build the rooms
            foreach (Room c in rooms)
            {
                //update position
                switch (c.dir)
                {
                    case RoomDir.Down: position.y -= (Room.roomDimensions.y + parameters.doorwaySize); break;
                    case RoomDir.Left: position.x -= (Room.roomDimensions.x + parameters.doorwaySize); break;
                    case RoomDir.Right: position.x += Room.roomDimensions.x + parameters.doorwaySize; break;
                }

                buildRoom(c, position, parameters);
            }
        }

        private void createDoors(ref List<Room> rooms, GeneratorParameters parameters)
        {
            Vector2D position = new Vector2D(0, 0);

            for (int i = 0; i < rooms.Count; ++i)
            {
                Room c = rooms[i];
                //build door between current room and the previous one
                buildDoor(c.dir, position, parameters, ref c, c.isLocked());

                //update position
                switch (c.dir)
                {
                    case RoomDir.STATIC:
                    case RoomDir.Down: position.y -= Room.roomDimensions.y + parameters.doorwaySize; break;
                    case RoomDir.Left: position.x -= Room.roomDimensions.x + parameters.doorwaySize; break;
                    case RoomDir.Right: position.x += Room.roomDimensions.x + parameters.doorwaySize; break;
                }

                if (i >= 3)
                {
                    // 3 previous rooms including the current one
                    List<Room> pRooms = rooms.GetRange(i - 3, 4);
                    //check if we can create a shortcut (if at none of the in between doors is locked)
                    bool canCutThrough = !pRooms[1].isLocked() && !pRooms[2].isLocked();
                    if (canCutThrough && Room.checkScore(pRooms.GetRange(1, 3)) == 0)
                    {
                        //build shortcut
                        Room r = pRooms[pRooms.Count - 1];
                        buildDoor(RoomDir.Up, position, parameters, ref r, r.isLocked());
                    }
                }
            }
        }

        //create a room, the given position is the top left corner of the room
        private void buildRoom(Room room, Vector2D position, GeneratorParameters parameters)
        {
            RectangleF pos = new RectangleF(position.x, position.y, Room.roomDimensions.x, Room.roomDimensions.y);
            ((ClassicMode)General.Editing.Mode).CenterOnArea(pos, 0f);

            PasteOptions options = new PasteOptions();
            CopyPasteManager copyPaste = General.Map.CopyPaste;

            using (FileStream stream = File.Open(room.prefabPath, FileMode.Open))
            {
                Vector2D dim = Room.roomDimensions;
                RectangleF rect = new RectangleF(position.x, position.y, dim.x, dim.y);

                ((ClassicMode)General.Editing.Mode).CenterOnArea(rect, 0f);

                copyPaste.InsertPrefabStream(stream, options);

                General.Map.Map.StitchGeometry();

                // Snap to map format accuracy
                General.Map.Map.SnapAllToAccuracy();

                // Clear selection
                General.Map.Map.ClearAllSelected();

                // Update cached values
                General.Map.Map.Update();

                // Update the used textures
                General.Map.Data.UpdateUsedTextures();

                // Map is changed
                General.Map.IsChanged = true;

                General.Interface.RedrawDisplay();

                //Ajoute, on enleve la marque sur les nouveaux secteurs
                General.Map.Map.ClearMarkedSectors(false);
            }

            if (room.type == RoomType.Key)
            {
                Thing t = addThing(position + Room.roomDimensions / 2);
                t.Type = parameters.nextKey();
            }
        }

        //position is top left corner of the room, dir refer to the position of the room compared to the previous one
        private void buildDoor(RoomDir dir, Vector2D position, GeneratorParameters parameters, ref Room room, bool locked = false)
        {
            if (dir != RoomDir.STATIC)
            {
                DrawnVertex v_c = new DrawnVertex();
                DrawnVertex v_c_door = new DrawnVertex();
                v_c.pos = v_c_door.pos = position;

                Vector2D dim = Room.roomDimensions;

                bool isHorizontalDoor = (dir == RoomDir.Left || dir == RoomDir.Right);

                switch (dir)
                {
                    case RoomDir.Left: //door at the right
                        v_c.pos.x -= parameters.doorwaySize;
                        v_c.pos.y += (parameters.doorwaySize + dim.y + dim.y / 2 - parameters.doorHeight / 2);
                        v_c_door.pos = v_c.pos;
                        v_c_door.pos.x += parameters.doorwaySize / 4f + parameters.doorSize / 2f;
                        break;
                    case RoomDir.Right: //door at the left
                        v_c.pos.x += dim.x;
                        v_c.pos.y += (parameters.doorwaySize + dim.y + dim.y / 2 - parameters.doorHeight / 2);
                        v_c_door.pos = v_c.pos;
                        v_c_door.pos.x += parameters.doorwaySize / 4f + parameters.doorSize / 2f;
                        break;
                    case RoomDir.Up: 
                        v_c.pos.y += 2 * dim.y + parameters.doorwaySize;
                        v_c.pos.x += (dim.x / 2 - parameters.doorHeight / 2);
                        v_c_door.pos = v_c.pos;
                        v_c_door.pos.y += (parameters.doorwaySize / 4f + parameters.doorSize / 2f);
                        break;
                    case RoomDir.STATIC:
                    case RoomDir.Down: //door at the top
                        v_c.pos.x += (dim.x / 2 - parameters.doorHeight / 2);
                        v_c.pos.y += dim.y;
                        v_c_door.pos = v_c.pos;
                        v_c_door.pos.y += (parameters.doorwaySize / 4f + parameters.doorSize / 2f);
                        break;
                }

                if (!isHorizontalDoor)
                {
                    newSector(v_c, parameters.doorHeight,
                        parameters.doorwaySize,
                        200,
                        parameters.doorHeight,
                        0);
                    newSector(v_c_door, parameters.doorHeight,
                        parameters.doorSize,
                        200,
                        parameters.doorHeight,
                        0);
                }
                else
                {
                    newSector(v_c, parameters.doorwaySize,
                        parameters.doorHeight,
                        200,
                        parameters.doorHeight,
                        0);
                    newSector(v_c_door, parameters.doorSize,
                        parameters.doorHeight,
                        200,
                        parameters.doorHeight,
                        0);
                }
                IList<Sector> sectors = General.Map.Map.Sectors as IList<Sector>;
                Sector s = sectors[sectors.Count - 1];
                s.SetCeilTexture("SLIME15");
                s.SetFloorTexture("SLIME14");
                s.CeilHeight -= parameters.doorHeight;
                int i = 0;
                bool includeNext = false;
                foreach (Sidedef side in s.Sidedefs)
                {
                    if(side.Line.Index % 2 == 0 && isHorizontalDoor || side.Line.Index % 2 != 0 && !isHorizontalDoor)
                    {
                        includeNext = true;
                    }
                    if(includeNext)
                    {
                        side.Line.FlipVertices();
                        side.SetTextureHigh("BIGDOOR4");
                        side.Line.Action = 1;
                        if (room.isLocked())
                        {
                            if (room.key != GeneratorParameters.Keys.NONE)
                                side.Line.Action = parameters.getLockValue(room.key);
                            else
                                side.Line.Action = parameters.nextLock(ref room.key);
                        }
                        else
                        {
                            side.Line.SetFlag(General.Map.Config.LowerUnpeggedFlag, true);
                        }
                        includeNext = false;
                    }
                }
                // Update the used textures
                General.Map.Data.UpdateUsedTextures();

                //re-draw sector to fix sectors being destoyed by sideDef flip
                if (!isHorizontalDoor)
                {
                    newSector(v_c, parameters.doorHeight,
                        parameters.doorwaySize,
                        200,
                        parameters.doorHeight,
                        0);
                }
                else
                {
                    newSector(v_c, parameters.doorwaySize,
                        parameters.doorHeight,
                        200,
                        parameters.doorHeight,
                        0);
                }
            }
            
        }
        
        //drawAnchor : Vertical > top, Horizontal > left
        /*
        private void buildDoor(bool isHorizontalDoor, Vector2D drawAnchor, int direction, GeneratorParameters parameters)
        {
            DrawnVertex v_c = new DrawnVertex();
            DrawnVertex v_c_door = new DrawnVertex();
            v_c.pos = v_c_door.pos = drawAnchor;

            switch(direction)
            {
                case 0: //droite
                    v_c_door.pos.x = v_c.pos.x + parameters.doorwaySize / 4f + parameters.doorSize / 2f;
                    break;
                case 1: //gauche
                    v_c.pos.x -= parameters.doorwaySize;
                    v_c_door.pos.x = v_c.pos.x + parameters.doorwaySize / 4f + parameters.doorSize / 2f;
                    break;
                case 2: //haut
                    v_c.pos.y += parameters.doorwaySize;
                    v_c_door.pos.y = v_c.pos.y - parameters.doorwaySize/ 4f - parameters.doorSize / 2f;
                    break;
                case 3: //bas
                    v_c_door.pos.y = v_c.pos.y + parameters.doorwaySize / 4f + parameters.doorSize / 2f;
                    break;
            }

            if (isHorizontalDoor)
            {
                newSector(v_c, parameters.doorHeight,
                    parameters.doorwaySize,
                    200,
                    parameters.doorHeight,
                    0);
                newSector(v_c_door, parameters.doorHeight,
                    parameters.doorSize,
                    200,
                    parameters.doorHeight,
                    0);
            }
            else
            {
                newSector(v_c, parameters.doorwaySize,
                    parameters.doorHeight,
                    200,
                    parameters.doorHeight,
                    0);
                newSector(v_c_door, parameters.doorSize,
                    parameters.doorHeight,
                    200,
                    parameters.doorHeight,
                    0);
            }
            IList<Sector> sectors = General.Map.Map.Sectors as IList<Sector>;
            Sector s = sectors[sectors.Count - 1];
            s.SetCeilTexture("SLIME15");
            s.SetFloorTexture("SLIME14");
            s.CeilHeight -= parameters.doorHeight;
            int i = 0;
            foreach (Sidedef side in s.Sidedefs)
            {
                if ((i % 2 != 0 && isHorizontalDoor) || (i%2 == 0 && !isHorizontalDoor))
                {
                    side.Line.FlipVertices();
                    side.SetTextureHigh("BIGDOOR4");
                    side.Line.Action = 1;
                }
                else
                {
                    side.Line.SetFlag(General.Map.Config.LowerUnpeggedFlag, true);
                }
                ++i;
            }
            // Update the used textures
            General.Map.Data.UpdateUsedTextures();

            //re-draw sector to fix sectors being destoyed by sideDef flip
            if(isHorizontalDoor)
            {
                newSector(v_c, parameters.doorHeight,
                    parameters.doorwaySize,
                    200,
                    parameters.doorHeight,
                    0);
            }
            else
            {
                newSector(v_c, parameters.doorwaySize,
                    parameters.doorHeight,
                    200,
                    parameters.doorHeight,
                    0);
            }
                
        }*/

        //return true if a position exist for the given room, update position check for the given position and room
        /*
        private bool checkNextPosition(Vector2D pv, Room room, out List<bool> positionCheck)
        {
            positionCheck = new List<bool>(4);

            //4 lines for raycasts
            Line2D l1 = new Line2D();
            Line2D l2 = new Line2D();
            Line2D l3 = new Line2D();
            Line2D l4 = new Line2D();

            Vector2D widthOffset = new Vector2D(((float) room.roomWidth)/2, 0f);
            Vector2D heightOffset = new Vector2D(0f, ((float)room.roomHeight) / 2);

            //A droite
            l1.v2 = l1.v1 = pv + heightOffset;
            l2.v2 = l2.v1 = pv - heightOffset;
            l3.v2 = l3.v1 = pv;
            l1.v2 = l1.v1 = pv;

            l4.v2.x = l2.v2.x = l1.v2.x += room.roomWidth;
            l3.v1 += new Vector2D((float)room.roomWidth, ((float)room.roomHeight) / 2);
            l3.v2 += new Vector2D((float)room.roomWidth, - ((float)room.roomHeight) / 2);

            positionCheck[0] = !(checkIntersect(l1) || checkIntersect(l2) || checkIntersect(l3) || checkIntersect(l4));


            //A gauche
            l1.v2 = l1.v1 = pv + heightOffset;
            l2.v2 = l2.v1 = pv - heightOffset;
            l3.v2 = l3.v1 = pv;
            l1.v2 = l1.v1 = pv;

            l4.v2.x = l2.v2.x = l1.v2.x -= room.roomWidth;
            l3.v1 += new Vector2D(-(float)room.roomWidth, ((float)room.roomHeight) / 2);
            l3.v2 += new Vector2D(-(float)room.roomWidth, - ((float)room.roomHeight) / 2);

            positionCheck[1] = !(checkIntersect(l1) || checkIntersect(l2) || checkIntersect(l3) || checkIntersect(l4));

            //En haut
            l1.v2 = l1.v1 = pv + widthOffset;
            l2.v2 = l2.v1 = pv - widthOffset;
            l3.v2 = l3.v1 = pv;
            l1.v2 = l1.v1 = pv;

            l4.v2.y = l2.v2.y = l1.v2.y += room.roomHeight;
            l3.v1 += new Vector2D(-((float)room.roomWidth) / 2, (float)room.roomHeight);
            l3.v2 += new Vector2D(((float)room.roomWidth) / 2, (float)room.roomHeight);

            positionCheck[2] = !(checkIntersect(l1) || checkIntersect(l2) || checkIntersect(l3) || checkIntersect(l4));

            //En bas
            l1.v2 = l1.v1 = pv + heightOffset;
            l2.v2 = l2.v1 = pv - heightOffset;
            l3.v2 = l3.v1 = pv;
            l1.v2 = l1.v1 = pv;

            l4.v2.y = l2.v2.y = l1.v2.y -= room.roomHeight;
            l3.v1 += new Vector2D(-((float)room.roomWidth) / 2, -(float)room.roomHeight);
            l3.v2 += new Vector2D(((float)room.roomWidth) / 2, -(float)room.roomHeight);

            positionCheck[3] = !(checkIntersect(l1) || checkIntersect(l2) || checkIntersect(l3) || checkIntersect(l4));

            bool oneDirOk = positionCheck[0] || positionCheck[1] || positionCheck[2] || positionCheck[3];

            return oneDirOk;
        }
    */

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void CreateRoomInfo_Click(object sender, EventArgs e)
        {
            serializePrefabInfo();
        }

        private void doorCheck_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void RoomType_t_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

[Serializable]
public enum RoomType{
    NONE,
    Key,        // contain a key
    Locked,     // doors are locked
    Boss,       // contain a Boss
    Basic,    // contain ennemies
    Spawn,
    End
}

//position relatiuve to the previous room
[Serializable]
public enum RoomDir
{
    STATIC , 
    Down ,
    Left ,
    Right,
    Up
}

[Serializable]
public class Room
{
    public Room()
    {

    }
    public Room(string prefabPath_t, RoomType type_t)
    {
        type = type_t;
        prefabPath = prefabPath_t;
    }
    public Room(Room c)
    {
        this.dir = c.dir;
        this.prefabPath = c.prefabPath;
        this.type = c.type;
    }
    public GeneratorParameters.Keys key = GeneratorParameters.Keys.NONE;
    public RoomType type;
    public RoomDir dir;
    public static Vector2D roomDimensions = new Vector2D(512,512);   
    public string prefabPath;

    public int getScore()
    {
        switch (dir)
        {
            case RoomDir.Left: return -1;
            case RoomDir.Right: return 1;
            default: return 0;
        }
    }

    public static int checkScore(List<Room> rooms)
    {
        int sum = 0;
        foreach(Room r in rooms) sum += r.getScore();
        return sum;
    }

    public bool isLocked()
    {
        return (type == RoomType.Locked);
    }
}

public class GeneratorParameters
{
    public int doorSize;
    public int doorwaySize;
    public int doorHeight;

    public enum Keys//26 27 28
    {
        BLUE_keycard,
        RED_keycard,
        YELLOW_keycard,
        BLUE_Skullkey,
        RED_Skullkey,
        YELLOW_Skullkey,
        NONE
    }

    public static Keys keyIndex = Keys.BLUE_keycard;
    public static Keys lockIndex = Keys.BLUE_keycard;

    //return the next key UID
    public int nextKey()
    {
        if ((int)keyIndex > 5) keyIndex = Keys.BLUE_keycard;
        int key = 0;
        switch (keyIndex)
        {
            case Keys.BLUE_keycard: key = 5; break;
            case Keys.RED_keycard: key = 13; break;
            case Keys.YELLOW_keycard: key = 6; break;
            case Keys.BLUE_Skullkey: key = 40; break;
            case Keys.RED_Skullkey: key = 38; break;
            case Keys.YELLOW_Skullkey: key = 39; break;
        }
        ++keyIndex; // todo
        return key;
    }

    //return the key value of the given key
    public int getKeyValue(Keys key)
    {
        int value = 0;
        switch (key)
        {
            case Keys.BLUE_keycard: value = 5; break;
            case Keys.RED_keycard: value = 13; break;
            case Keys.YELLOW_keycard: value = 6; break;
            case Keys.BLUE_Skullkey: value = 40; break;
            case Keys.RED_Skullkey: value = 38; break;
            case Keys.YELLOW_Skullkey: value = 39; break;
        }
        return value;
    }

    //return the next lock UID
    public int nextLock(ref Keys keyToAlter)
    {
        if ((int)lockIndex > 5) lockIndex = Keys.BLUE_keycard;
        keyToAlter = lockIndex;
        int key = 0;
        switch (lockIndex)
        {
            case Keys.BLUE_keycard: key = 26; break;
            case Keys.RED_keycard: key = 28; break;
            case Keys.YELLOW_keycard: key = 27; break;
            case Keys.BLUE_Skullkey: key = 26; break;
            case Keys.RED_Skullkey: key = 28; break;
            case Keys.YELLOW_Skullkey: key = 27; break;
        }
        ++lockIndex; // todo
        return key;
    }

    //return the lock value of the given key
    public int getLockValue(Keys key)
    {
        int value = 0;
        switch (key)
        {
            case Keys.BLUE_keycard: value = 26; break;
            case Keys.RED_keycard: value = 28; break;
            case Keys.YELLOW_keycard: value = 27; break;
            case Keys.BLUE_Skullkey: value = 26; break;
            case Keys.RED_Skullkey: value = 28; break;
            case Keys.YELLOW_Skullkey: value = 27; break;
        }
        return value;
    }

}

public class LDGenerationException : Exception
{
    public LDGenerationException() { }
    public LDGenerationException(string msg) : base(msg) { }
    public LDGenerationException(string msg, Exception inner) : base(msg, inner) { }
}
