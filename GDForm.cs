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

        private List<Room> deserializePrefabsInfo()
        {
            List<Room> rooms = new List<Room>();
            bool readAllFiles = false;
            int iterationCount = 0;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Room));

            while (!readAllFiles)
            {
                string path = "prefabs/room" + iterationCount + ".json";

                if (File.Exists(path))
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
            List<Dictionary<int, Vector2D>> doorPositions_t = new List<Dictionary<int, Vector2D>>();
            Vector2D[] positions = new Vector2D[4];

            positions[0] = new Vector2D(float.Parse(R_x.Text), float.Parse(R_y.Text));
            positions[1] = new Vector2D(float.Parse(L_x.Text), float.Parse(L_y.Text));
            positions[2] = new Vector2D(float.Parse(T_x.Text), float.Parse(T_y.Text));
            positions[3] = new Vector2D(float.Parse(B_x.Text), float.Parse(B_y.Text));
           
            for (int x = 0; x <= doorCheck.CheckedIndices.Count - 1; x++)
            {
                int indice = Convert.ToInt32(doorCheck.CheckedIndices[x].ToString());
                Dictionary<int, Vector2D> position = new Dictionary<int, Vector2D>();
                position.Add(indice, positions[indice]);
                doorPositions_t.Add(position);
            }
            
            Room newRoom = new Room()
            {
                roomHeight = Convert.ToInt32(roomHeight.Text),
                roomWidth = Convert.ToInt32(roomWidth.Text),
                prefabPath = prefabObjectPath.Text,
                type = (RoomType)Enum.Parse(typeof(RoomType), RoomType.SelectedItem.ToString(), true),
                doorPositions = doorPositions_t
            };

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
            makeOnePath(true);
            makeOnePath(false);
            makeOnePath(false);

            correctMissingTex();
        }

        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            showCategories();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            generateLD(Convert.ToInt32(roomNumber.Text), Convert.ToInt32(doorWidth.Text), Convert.ToInt32(doorSize.Text), Convert.ToInt32(doorHeight.Text));

            correctMissingTex();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void generateLD(int roomNb, int dWidth, int dSize, int dHeight)
        {
            GeneratorParameters parameters = new GeneratorParameters();
            parameters.rooms = deserializePrefabsInfo();
            parameters.doorwaySize = dWidth;
            parameters.doorSize = dSize;
            parameters.doorHeight = dHeight;

            createRoom(0,new Vector2D(), new Room(), parameters); // create first room and other recursively
        }

        private void createRoom(int previousDir, Vector2D position, Room room, GeneratorParameters parameters)
        {
            Random r = new Random();
            List<bool> checkPositions = new List<bool>(4);

            int nextDir = previousDir;
            bool oneDirOk = checkNextPosition(position, new Room(), out checkPositions);
            if (!oneDirOk)
                Console.WriteLine("No more path :(");
            else
            {
                int nbTry = 0;
                while ((!checkPositions[nextDir] || previousDir == nextDir) && nbTry++ < 100)
                    nextDir = r.Next() % 4;
            }
        }
        
        //drawAnchor : Vertical > top, Horizontal > left
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
                
        }

        //return true if a position exist for the given room, update position check for the given position and room
        private bool checkNextPosition(Vector2D pv, Room room, out List<bool> positionCheck)
        {
            positionCheck = new List<bool>(4);

            //4 lines for raycasts
            /* (ex: RIGHT)
             * -----|
             * -----|
             * -----|
             */
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void CreateRoomInfo_Click(object sender, EventArgs e)
        {
            serializePrefabInfo();
        }
    }
}

public enum RoomType{
    Key,        // contain a key
    Locked,     // doors are locked
    Boss,       // contain a Boss
    Normal    // contain ennemies
}

public class Room
{
    public RoomType type;
    public Vector2D roomDimensions;
    public string prefabPath;
}

public class GeneratorParameters
{
    public int doorSize;
    public int doorwaySize;
    public int doorHeight;

    public List<Room> rooms; // loaded rooms
}
