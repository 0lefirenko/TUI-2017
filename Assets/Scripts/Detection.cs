using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

public class Detection : MonoBehaviour {
    Vector2 size = new Vector2(1000, 1000);
    public Texture2D _texture;
    public struct Corner
    {
        public int X;
        public int Y;
        public int V;
    }
    public struct Element
    {
        public int X;
        public int Y;
        public int N;
    }

    public Corner[] corners = new Corner[5000];
    public Element[] elements = new Element[500];
    public int cornersAmount = 0, elementsAmount = 0;
    public int elementMaxSize = 0;
    public int[,] finalDiagram = new int[50, 50];

    public int[,] colorArray = new int[1000, 1000];
    public int[,] diagram = new int[1000, 1000];
    //public int[,] colorScheme = new int[2500, 2500];
    public int[,] finalScheme = new int[1000, 1000];
    //public static Texture2D _texture = null;
    public int leftBorder, rightBorder, topBorder, bottomBorder;
    public int cell_size;

    string playerDataPath = "", path = "temp";
    public static void begin()
    {
       
    }
    void Start()
    {
       

    }
    
	void Update () {
        if (Input.GetMouseButtonDown(2))
        {
            open();

            playerDataPath = "D:/" + path + ".db";
            elementMaxSize = max(_texture.width, _texture.height) * 80 / 337;
           

            setDiagram();
            findElement();
            //cellSize = max(_texture.width / 12, _texture.height / 12);
            // playerDataPath = "D:/" + "SchemeFromPhotot" + ".db";
            setCornersOnScheme();
            setScheme();
            setWires();
            Save();
            SaveLoadScript.playerDataPath = playerDataPath;
            SaveLoadScript.Load();
            //setBorders();

        }
    }

    void setDiagram()
    {
        setColors();
        setCorner();
    }

    //--------------------------------------------------------------------------------------------------------------
    void setColors()
    {
        for (int i = 0; i < _texture.width; i++)
        {
            for (int j = _texture.height; j > 0; j--)
            {
                //if (_texture.GetPixel(i, j) != Color.black && _texture.GetPixel(i, j)!= Color.white) Debug.Log(_texture.GetPixel(i, j));
                if (_texture.GetPixel(i, j).r <= 100* 1.0/ 255 && _texture.GetPixel(i, j).g <= 100*1.0 / 255 && _texture.GetPixel(i, j).b <= 100*1.0/255)
                {
                    colorArray[i, _texture.height - j] = 1;
                }
                else { colorArray[i, _texture.height - j] = 0; }
            }
        }
        
    }
  
    //--------------------------------------------------------------------------------------------------------------------------
    public void setCorner()
    {
        for (int j = 10; j < _texture.height; j++)
        {
            for (int i = 10; i < _texture.width; i++)
            {
                if (getCorner(i, j) != 0)
                {
                    cornersAmount++;
                    corners[cornersAmount].X = i;
                    corners[cornersAmount].Y = j;
                    corners[cornersAmount].V = getCorner(i, j);
                    //Debug.Log(i + "    " + j + "    " + getCorner(i, j));
                }
            }
        }
    }

    public int getCorner(int x, int y)
    {
      
        if (if1(x, y) == true) return 1;
        if (if2(x, y) == true) return 2;
        if (if3(x, y) == true) return 3;
        if (if4(x, y) == true) return 4;

        return 0;

    }
    public bool if1(int x, int y)
    {
        if (colorArray[x + 1, y + 1] == 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x + i, y] != 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x, y + i] != 1) return false;
        return true;
    }
    public bool if2(int x, int y)
    {
        if (y > 1) if (colorArray[x + 1, y - 1] == 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x + i, y] != 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x, y - i] != 1) return false;
        return true;
    }
    public bool if3(int x, int y)
    {
        if (x > 0 && y > 0) if (colorArray[x - 1, y - 1] == 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x - i, y] != 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x, y - i] != 1) return false;
        return true;
    }
    public bool if4(int x, int y)
    {
        if (x > 0) if (colorArray[x - 1, y + 1] == 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x - i, y] != 1) return false;
        for (int i = 0; i < 5; i++) if (colorArray[x, y + i] != 1) return false;
        return true;
    }
    //----------------------------------------------------------------------------------------------------
    //------------Work with corners-----------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------

    void sortCorners()
    {
        for (int i = 1; i <= cornersAmount; i++)
        {
            for (int j = 1; j <= cornersAmount - i ; j++)
            {
                if (corners[j].V > corners[j + 1].V)
                {
                    int temp = corners[j].X;
                    corners[j].X = corners[j + 1].X;
                    corners[j+1].X = temp;

                    temp = corners[j].Y;
                    corners[j].Y = corners[j + 1].Y;
                    corners[j + 1].Y = temp;

                    temp = corners[j].V;
                    corners[j].V = corners[j + 1].V;
                    corners[j + 1].V = temp;
                }
            }
        }
        /*for (int i = 1; i <= cornersAmount; i++)
        {
            Debug.Log(corners[i].X + "    " + corners[i].Y + "    " + corners[i].V);
        }*/
    }
    void findElement()
    {
        sortCorners();
        for (int k = 1; k <= cornersAmount; k++)
        {

            if (corners[k].V == 1)
            {
                //rezistor
                int i = 0, j = 0;
                int amm = 0;

                for (i = 1; i <= cornersAmount; i++)
                {
                    if (corners[i].V == 2 && abs(corners[k].X - corners[i].X) < 5 && corners[i].Y - corners[k].Y < elementMaxSize && corners[i].Y - corners[k].Y > 0) { amm++; break; }
                }
                for (j = 1; j <= cornersAmount; j++)
                {
                    if (corners[j].V == 4 && (abs(corners[j].Y - corners[k].Y) < 5) && corners[j].X - corners[k].X < elementMaxSize && corners[j].X - corners[k].X > 0) {  amm++; break; }
                }

                if (amm == 2)
                {
                    for (int z = 1; z <= cornersAmount; z++)
                        if (corners[z].V == 3 && abs(corners[z].X - corners[j].X) < 3 && abs(corners[z].Y - corners[i].Y) < 3)
                        {
                           Debug.Log("rezistor  " + (corners[k].X + corners[j].X) / 2 + "      " + (corners[k].Y + corners[i].Y) / 2);

                            elementsAmount++;
                            elements[elementsAmount].X = (corners[k].X + corners[j].X) / 2;
                            elements[elementsAmount].Y = (corners[k].Y + corners[i].Y) / 2;
                            elements[elementsAmount].N = 6;
                            corners[i].V = -1;
                            corners[j].V = -1;
                            corners[z].V = -1;
                            corners[k].V = -1;
                            int halfX = (corners[k].X + corners[j].X) / 2, halfY = (corners[k].Y + corners[i].Y) / 2;
                            for (int v = 0; v <= cornersAmount; v++)
                                if ((corners[v].X - halfX) * (corners[v].X - halfX) + (corners[v].Y - halfY) * (corners[v].Y - halfY) <= elementMaxSize * elementMaxSize / 2 ) corners[v].V = -1;

                            
                        }
                }
            }
        }
        for (int k = 1; k <= cornersAmount; k++)
        { 
            //eds
            if (corners[k].V == 3)
            {
                int i = 0, j = 0, amm = 0;
                for (i = 1; i < cornersAmount; i++)
                {
                    if (corners[i].V != -1) Debug.Log(corners[i].X + "  " + corners[i].Y + "  " + corners[i].V);
                }
                for (i = 1;i <= cornersAmount; i++) 
                    if (abs(corners[i].X - corners[k].X) < 3 && corners[i].Y - corners[k].Y <= elementMaxSize && corners[i].Y - corners[k].Y > 0 && corners[i].V == 4) { amm++; break; }
                for (j = 1; j <= cornersAmount; j++)
                    if (abs(corners[j].Y - corners[k].Y) < 3 && corners[j].X - corners[k].X <= elementMaxSize && corners[j].X - corners[k].X > 0 && corners[j].V == 2) { amm++; break; }
               
                if (amm == 2)
                {
                   Debug.Log("EDS " + corners[k].X + "  " + corners[k].Y);
                    elementsAmount++;
                    elements[elementsAmount].X = (corners[k].X + corners[j].X) / 2;
                    elements[elementsAmount].Y = (corners[k].Y + corners[i].Y) / 2;
                    elements[elementsAmount].N = 14;
                    if (corners[j].X - corners[k].X < corners[i].Y - corners[k].Y) elements[elementsAmount].N = -14;

                    corners[k].V = -1;
                    corners[i].V = -1;
                    corners[j].V = -1;
                    for (int z = 0; z<= cornersAmount; z++)
                    {
                        if (abs(corners[z].X - corners[j].X)<3 && abs(corners[z].Y - corners[i].Y) < 3 && corners[z].V == 1)  corners[z].V = -1;
                    }
                }

            }
        }
        for (int k = 1; k <= cornersAmount; k++)
        {
            int i = 0, j = 0;
            //buzzer
            if (corners[k].V == 4)
            {
                for (i = 1; i<= cornersAmount; i++)
                    if (corners[i].Y == corners[k].Y && corners[k].X - corners[i].X <= elementMaxSize/10 && corners[k].X - corners[i].X > 0 && corners[i].V == 1)
                    { 
                            Debug.Log("buzzer");
                        elementsAmount++;
                        elements[elementsAmount].X = (corners[k].X + corners[i].X) / 2;
                        elements[elementsAmount].Y = corners[k].Y;
                        elements[elementsAmount].N = 19;
                            corners[i].V = -1;
                            corners[k].V = -1;
                        int halfX = (corners[k].X + corners[j].X) / 2, halfY = (corners[k].Y + corners[i].Y) / 2;
                        for (int v = 0; v <= cornersAmount; v++)
                            if ((corners[v].X - halfX) * (corners[v].X - halfX) + (corners[v].Y - halfY) * (corners[v].Y - halfY) <= elementMaxSize * elementMaxSize / 8) corners[v].V = -1;



                    }

                for (j = 1; j <= cornersAmount; j++)
                    if (corners[j].X == corners[k].X && corners[j].X - corners[k].X <= elementMaxSize && corners[j].X - corners[k].X > 0 && corners[j].V == 2)
                    {
                        Debug.Log("-buzzer");
                        elementsAmount++;
                        elements[elementsAmount].X = (corners[k].Y + corners[i].Y) / 2;
                        elements[elementsAmount].Y = corners[k].X;
                        elements[elementsAmount].N = -19;
                        corners[j].V = -1;
                        corners[k].V = -1;
                        int halfX = (corners[k].X + corners[j].X) / 2, halfY = (corners[k].Y + corners[i].Y) / 2;
                        for (int v = 0; v <= cornersAmount; v++)
                            if ((corners[v].X - halfX) * (corners[v].X - halfX) + (corners[v].Y - halfY) * (corners[v].Y - halfY) <= elementMaxSize * elementMaxSize / 4) corners[v].V = -1;

                    }
            }
        }
        /*for (int i = 1; i <= elementsAmount; i++)
        {
            Debug.Log("" + elements[i].X + "  " + elements[i].Y + " " + elements[i].N);
        }*/
        
    }
    int abs(int a)
    {
        if (a < 0) return -1 * a;
        return a;
    }
    int max(int a, int b)
    {
        if (a > b) return a; else return b;
    }
    int min(int a, int b)
    {
        if (a < b) return a; else return b;
    }
    void setCornersOnScheme()
    {
        for (int i = 1; i <= cornersAmount; i++)
        {
            if (corners[i].V != -1)
            {
                elementsAmount++;
                elements[elementsAmount].X = corners[i].X;
                elements[elementsAmount].Y = corners[i].Y;
                elements[elementsAmount].N = corners[i].V;
            }
        }
    }
    void setScheme()
    {
        for (int i = 1; i <= elementsAmount; i++)
        {
            finalDiagram[round(elements[i].X * 14.0 / _texture.width) , round(elements[i].Y * 11.0 / _texture.height)] = elements[i].N;
        }
    }
  
    void setWires()
    {
        
        for (int j = 0; j < 11; j++)
        {
            for (int i = 0; i < 14; i++)
            {
                     
                if (finalDiagram[i,j] == 1)
                {
                    for (int z = 1; z <= 15; z++)
                    {
                        if (finalDiagram[i + z, j] <= 4 && finalDiagram[i + z, j] >= 1) break;
                        if (finalDiagram[i + z, j] == 0) finalDiagram[i + z, j] = 10;
                    }

                    for (int z = 1; z <= 15; z++)
                    {
                        if (finalDiagram[i, j + z] <= 4 && finalDiagram[i, j + z] >= 1) break;
                        if (finalDiagram[i, j + z] == 0) finalDiagram[i, j + z] = -10;
                    }
                }

                if (finalDiagram[i, j] == 3)
                {
                    for (int z = 1; z < i; z++)
                    {
                        if (finalDiagram[i - z, j] <= 4 && finalDiagram[i - z, j] >= 1) break;
                        if (finalDiagram[i - z, j] == 0) finalDiagram[i - z, j] = 10;
                    }

                    for (int z = 1; z < j; z++)
                    {
                        if (finalDiagram[i, j - z] <= 4 && finalDiagram[i, j - z] >= 1) break;
                        if (finalDiagram[i, j - z] == 0) finalDiagram[i, j - z] = -10;
                    }
                }
                if (finalDiagram[i, j] == 2)
                {
                    for (int z = 1; z < i; z++)
                    {
                        if (finalDiagram[i + z, j] <= 4 && finalDiagram[i + z, j] >= 1) break;
                        if (finalDiagram[i + z, j] == 0) finalDiagram[i + z, j] = 10;
                    }

                    for (int z = 1; z < j; z++)
                    {
                        if (finalDiagram[i, j - z] <= 4 && finalDiagram[i, j - z] >= 1) break;
                        if (finalDiagram[i, j - z] == 0) finalDiagram[i, j - z] = -10;
                    }
                }
                if (finalDiagram[i, j] == 4)
                {
                    for (int z = 1; z < i; z++)
                    {
                        if (finalDiagram[i - z, j] <= 4 && finalDiagram[i - z, j] >= 1) break;
                        if (finalDiagram[i - z, j] == 0) finalDiagram[i - z, j] = 10;
                    }

                    for (int z = 1; z < j; z++)
                    {
                        if (finalDiagram[i, j + z] <= 4 && finalDiagram[i, j + z] >= 1) break;
                        if (finalDiagram[i, j + z] == 0) finalDiagram[i, j + z] = -10;
                    }
                }
            }
        }
    }

    void Save()
    {
        /*for (int j = 0; j < 11; j++)
            for (int i = 0; i < 14; i++)
                Debug.Log(i + "   " + j + "    " + finalDiagram[i, j]);*/

        /*for (int i = 1; i <= cornersAmount; i++)
        {
            Debug.Log(corners[i].X + " " + corners[i].Y + "   " + corners[i].V);
        }*/
                StreamWriter dataWriter = new StreamWriter(playerDataPath);
        
        for (int j = 0; j < 11; j++) 
        {
            for (int i = 0; i < 14; i++)
            {
                switch(finalDiagram[i, 10 - j])
                {
                    case 0:
                        dataWriter.WriteLine(i + " " + j + " " + "-1" + " " + "1 0 0 0 -1 -1");//wireangel
                        break;
                    case 1:
                        dataWriter.WriteLine(i + " " + j + " " + "0" + " " + "1 0 0 0 -1 -1");//wireangel
                        break;
                    case 2:
                        dataWriter.WriteLine(i + " " + j + " " + "0" + " 0.7071068 0 0 0.7071068 -1 -1");//wireangel
                        break;
                    case 3:
                        dataWriter.WriteLine(i + " " + j + " " + "0" + " 0 0 0 1 -1 -1");//wireangel
                        break;
                    case 4:
                        dataWriter.WriteLine(i + " " + j + " " + "0" + " -0.7071068 0 0 0.7071068 -1 -1");//wireangel
                        break;
                    case -10:
                        dataWriter.WriteLine(i + " " + j + " " + "8" + " " + "1 0 0 0 -1 -1");//wire
                        break;
                    case 10:
                        dataWriter.WriteLine(i + " " + j + " " + "8" + " " + "0.7071068 0 0 0.7071068 -1 -1");//wire
                        break;
                    case 14:
                         dataWriter.WriteLine(i + " " + j + " " + "3" + " " + "0 0 0 1 -1 -1");//battery
                        break;
                    case -14:
                        dataWriter.WriteLine(i + " " + j + " " + "3" + " " + "0.7071068 0 0 0.7071068 -1 -1");//battery
                        break;
                    case 6:
                        if (finalDiagram[i + 1, 10 - j] == 1 || finalDiagram[i-1,10-j] == 1 ) dataWriter.WriteLine(i + " " + j + " " + "6" + " " + "0.7071068 0 0 0.7071068 -1 -1");
                            else dataWriter.WriteLine(i + " " + j + " " + "6" + " " + "0 0 0 1 -1 -1");
                        //rezistor
                        break;
                    case -19:
                        dataWriter.WriteLine(i + " " + j + " " + "4" + " " + "0.7071068 0 0 0.7071068 -1 -1");//battery
                        break;
                    case 19:
                        dataWriter.WriteLine(i + " " + j + " " + "4" + " " + "0 0 0 1 -1 -1");//battery
                        break;



                }

                /*if (finalDiagram[i, j] == 0) dataWriter.WriteLine(i + " " + j + " " + "-1" + " " + "1 0 0 0 -1 -1");
                else
                    if (finalDiagram[i, j] <= 4 && finalDiagram[i, j] >= 1) dataWriter.WriteLine(i + " " + j + " " + "0 " + "1 0 0 0 -1 -1");
                     else
                           if (finalDiagram[i, j] == 10)
                                { dataWriter.WriteLine(i + " " + j + " " + "8" + " " + "1 0 0 0 -1 -1"); }  
                                else
                                     if (finalDiagram[i, j] == -10)
                                        { dataWriter.WriteLine(i + " " + j + " " + "8" + " " + "-0,7071068f 0 0 0,7071068f -1 -1"); }    
                                        else dataWriter.WriteLine(i + " " + j + " " + finalDiagram[i, j] + " " + "1 0 0 0 -1 -1");  */      
            }
        }

        /* for (int j = 0; j < 12; j++)
         {
             for (int i = 0; i < 12; i++)
             {
                if ((finalScheme[i, j] == 5 || finalScheme[i, j] == 6) && finalScheme[i, j - 1] != 0)
                      dataWriter.WriteLine(i + " " + j + " " + finalScheme[i, j]/3 + " " + "-0,7071068f 0 0 0,7071068f -1 -1");
                 if ((finalScheme[i, j] == 5 || finalScheme[i, j] == 6) && finalScheme[i - 1, j] != 0)
                     dataWriter.WriteLine(i + " " + j + " " + finalScheme[i, j]/3 + " " + "1 0 0 0 -1 -1");
                 switch (finalScheme[i,j])
                 {
                     case 1:
                         dataWriter.WriteLine(i + " " + j + " " + 0 + " " + "1 0 0 0 -1 -1");
                     break;
                     case 2:
                         dataWriter.WriteLine(i + " " + j + " " + 0 + " " + "-0,7071068f 0 0 0,7071068f -1 -1");
                         break;
                     case 3:
                         dataWriter.WriteLine(i + " " + j + " " + 0 + " " + "0f 0 0 1f -1 -1");
                         break;
                     case 4:
                         dataWriter.WriteLine(i + " " + j + " " + 0 + " " + "0,7071068f 0 0 0,7071068f -1 -1");
                         break;
                     case 10:
                         dataWriter.WriteLine(i + " " + j + " " + 8 + " " + "1 0 0 0 -1 -1");
                         break;
                     case -10:
                         dataWriter.WriteLine(i + " " + j + " " + 8 + " " + "-0,7071068f 0 0 0,7071068f -1 -1");
                         break;
                     case 0:
                         dataWriter.WriteLine(i + " " + j + " " + 5 + " " + "1 0 0 0 -1 -1");
                     break;


                 }

                 // dataWriter.WriteLine(i + " " + j + " " + finalScheme[i, j] + " "  );
             }

         }*/
        dataWriter.Flush();
        dataWriter.Close();
    }
    int round( double n)
    {
        n *= 10;
        if (n % 10 > 5) n += 10;
        return (int)n/10;
    }

    void open()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "photos (*.png)|*.png";
        ofd.ShowDialog();

       
        Debug.Log("" + ofd.FileName);

        _texture = loadImage(ofd.FileName);
        
        return;
    }
    private static Texture2D loadImage(string filePath)
    {

        byte[] bytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(1000, 1000, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Trilinear;
        texture.LoadImage(bytes);

        return texture;
    }
}