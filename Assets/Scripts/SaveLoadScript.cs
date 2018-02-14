using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class SaveLoadScript : MonoBehaviour
{
    Gauss.ComplexDouble[,] a_matrix = new Gauss.ComplexDouble[100, 100];
    Gauss.ComplexDouble[] b_vector = new Gauss.ComplexDouble[100];
    int[] colorArray = new int[100];
    public static int countColor = 0;
    double[] result = new double[100];
    public static bool doAnalizeWindow;
    public static double[,] answerMatrix = new double[100, 100];
    public GameObject arrowUp, arrowLeft, arrowRight, arrowDown, potentialSprite;
    int xPosEds, yPosEds;

    public struct ResistorInOut
    {
        public int firstUzel;
        public int secondUzel;
        public float resistorNum;
    };
    public ResistorInOut[,] resistorsMatrixArr = new ResistorInOut[100, 100];
    private int boundColor = 0;
    private int[,] boardColor = new int[100, 100];
    [SerializeField]
    public Sprite[] arraySprites;

    public int getIntOfImage(Sprite sprite1)
    {

        for (int i = 0; i < arraySprites.Length; i++)
        {
            if (arraySprites[i] == sprite1)
            {
                return i;
            }
        }
        return -1;
    }
    double electricity = -1;
    public static string playerDataPath = "", path = "";
    public static float[,] Saves = new float[155, 9];
    public static float[,,] matrix = new float[100, 100, 9];

    void dfsWire(int i, int j, int color)
    {

        if (matrix[i, j, 2] != -1)
            boardColor[i, j] = color;
        if (matrix[i, j, 2] == 8)
        {
            if ((matrix[i, j, 3] == 1 && matrix[i, j, 6] == 0) || (matrix[i, j, 3] == 0 && matrix[i, j, 6] == 1))
            {
                //Debug.Log(i + " " + j + " " + 1);
                if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
                {
                    dfsWire(i, j - 1, color);
                }
                if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
                {
                    dfsWire(i, j + 1, color);
                }
            }
            else
            if ((matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f) || (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f))
            {
                //Debug.Log(i + " " + j + " " + 2);
                if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
                {
                    dfsWire(i - 1, j, color);
                }
                if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
                {
                    dfsWire(i + 1, j, color);
                }
            }
        } else
        if (matrix[i, j, 2] == 1)
        {
            if ((matrix[i, j, 3] == 1 && matrix[i, j, 6] == 0) || (matrix[i, j, 3] == 0 && matrix[i, j, 6] == 1))
            {
                //Debug.Log(i + " " + j + " " + 1);
                if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
                {
                    dfsWire(i - 1, j, color);
                }
                if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
                {
                    dfsWire(i + 1, j, color);
                }
            }
            else
            if ((matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f) || (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f))
            {
                //Debug.Log(i + " " + j + " " + 2);
                if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
                {
                    dfsWire(i, j - 1, color);
                }
                if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
                {
                    dfsWire(i, j + 1, color);
                }
            }
        }
        else
        if (matrix[i, j, 2] == 0)
        {
            if (matrix[i, j, 3] == 1f && matrix[i, j, 6] == 0f)
            {
                //Debug.Log(i + " " + j + " " + 1);
                if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
                {
                    dfsWire(i, j - 1, color);
                }
                if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
                {
                    dfsWire(i + 1, j, color);
                }
            }
            else
            if (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                //Debug.Log(i + " " + j + " " + 2);
                if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
                {
                    dfsWire(i - 1, j, color);
                }
                //if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
                {
                    dfsWire(i, j - 1, color);
                }
            }
            else
            if (matrix[i, j, 3] == 0f && matrix[i, j, 6] == 1f)
            {
                //Debug.Log(i + " " + j + " " + 3);
                if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
                {
                    dfsWire(i, j + 1, color);
                }
                if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
                {
                    dfsWire(i - 1, j, color);
                }
            }
            else
            if (matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                //Debug.Log(i + " " + j + " " + 4);
                if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
                {
                    dfsWire(i + 1, j, color);
                }
                if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
                {
                    dfsWire(i, j + 1, color);
                }
            }
        }
        else
        if (matrix[i, j, 2] == 10)
        {
            if (matrix[i, j, 3] == 1f && matrix[i, j, 6] == 0f)
            {
                //Debug.Log(i + " " + j + " " + 1);
                if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
                {
                    dfsWire(i, j - 1, color);
                }
                if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
                {
                    dfsWire(i, j + 1, color);
                }
                if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
                {
                    dfsWire(i + 1, j, color);
                }
            }
            else
            if (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                //Debug.Log(i + " " + j + " " + 2);
                if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
                {
                    dfsWire(i - 1, j, color);
                }
                if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
                {
                    dfsWire(i + 1, j, color);
                }
                if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
                {
                    dfsWire(i, j - 1, color);
                }
            }
            else
            if (matrix[i, j, 3] == 0f && matrix[i, j, 6] == 1f)
            {
                //Debug.Log(i + " " + j + " " + 3);
                if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
                {
                    dfsWire(i, j + 1, color);
                }
                if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
                {
                    dfsWire(i, j - 1, color);
                }
                if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
                {
                    dfsWire(i - 1, j, color);
                }
            }
            else
            if (matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                //Debug.Log(i + " " + j + " " + 4);
                if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
                {
                    dfsWire(i + 1, j, color);
                }
                if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
                {
                    dfsWire(i - 1, j, color);
                }
                if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
                {
                    dfsWire(i, j + 1, color);
                }
            }
        }
        else
        if (matrix[i, j, 2] == 9)
        {
            if (boardColor[i + 1, j] != color && boardColor[i + 1, j] > -1)
            {
                dfsWire(i + 1, j, color);
            }
            if (boardColor[i - 1, j] != color && boardColor[i - 1, j] > -1)
            {
                dfsWire(i - 1, j, color);
            }
            if (boardColor[i, j + 1] != color && boardColor[i, j + 1] > -1)
            {
                dfsWire(i, j + 1, color);
            }
            if (boardColor[i, j - 1] != color && boardColor[i, j - 1] > -1)
            {
                dfsWire(i, j - 1, color);
            }
        }
        else if (matrix[i, j, 2] == 3 || matrix[i, j, 2] == 6 || matrix[i, j, 2] == 4 || matrix[i, j, 2] == 1 || matrix[i, j, 2] == 2 || matrix[i, j, 2] == 12 || matrix[i, j, 2] == 11) dfs(i, j);
    }

    private double amperage = 0;
    private bool[,] isVisited = new bool[14,11];
    private void Initialized()
    {
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                isVisited[i,j] = false;
            }
        }

    }

    void Amperage(int i, int j)
    {
        Debug.Log(i + " " +  j);
        isVisited[i, j] = true;
        if (matrix[i, j, 2] == 6 || matrix[i, j, 2] == 4)
        {
            amperage += answerMatrix[i, j];
        } else
        if (matrix[i, j, 2] == 1)
        {

            if (matrix[i, j, 3] == 1f && matrix[i, j, 6] == 0f)
            {
                Amperage(i - 1, j);
            }
            else
            if (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                Amperage(i, j + 1);
            }
            else
            if (matrix[i, j, 3] == 0f && matrix[i, j, 6] == 1f)
            {
                Amperage(i + 1, j);
            }
            else
            if (matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                Amperage(i, j - 1);
            }
        } else
        if (matrix[i, j, 2] == 3)
        {
            if (matrix[i, j, 3] == 1f && matrix[i, j, 6] == 0f)
            {
                Amperage(i + 1, j);
            }
            else
            if (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                Amperage(i, j - 1);
            }
            else
            if (matrix[i, j, 3] == 0f && matrix[i, j, 6] == 1f)
            {
                Amperage(i - 1, j);
            }
            else
            if (matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                Amperage(i, j + 1);
            }
        }
        else
        if (boardColor[i,j] != -1)
        {
            if ((boardColor[i+1, j] == boardColor[i, j] || boardColor[i + 1, j] == -6 || boardColor[i + 1, j] == -3) && !isVisited[i+1, j])
            {
                Amperage(i + 1, j);
            }
            if ((boardColor[i - 1, j] == boardColor[i, j] || (boardColor[i - 1, j] == -6 || boardColor[i + 1, j] == -3)) && !isVisited[i - 1, j])
            {
                Amperage(i - 1, j);
            }
            if ((boardColor[i, j + 1] == boardColor[i, j] || (boardColor[i, j + 1] == -6 || boardColor[i + 1, j] == -3)) && !isVisited[i, j + 1])
            {
                Amperage(i, j + 1);
            }
            if ((boardColor[i, j - 1] == boardColor[i, j] || boardColor[i, j - 1] == -6 || boardColor[i + 1, j] == -3)  && !isVisited[i, j - 1])
            {
                Amperage(i, j - 1);
            }
        }
    }
    void dfs(int i, int j)
    {
        if (matrix[i, j, 2] == 3)
        {
            boardColor[i, j] = -3;
            if (matrix[i, j, 3] == 1f && matrix[i, j, 6] == 0f)
            {
                boundColor++;
                dfs(i + 1, j);
            }
            else
            if (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                boundColor++;
                dfs(i, j - 1);
            }
            else
            if (matrix[i, j, 3] == 0f && matrix[i, j, 6] == 1f)
            {
                boundColor++;
                dfs(i - 1, j);
            }
            else
            if (matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f)
            {
                boundColor++;
                dfs(i, j + 1);
            }
        }
        else
        if (matrix[i, j, 2] == 6 || matrix[i, j, 2] == 4 || matrix[i, j, 2] == 12 || matrix[i, j, 2] == 11)
        {
            if (matrix[i, j, 2] == 6) boardColor[i, j] = -6;
            if (matrix[i, j, 2] == 4) boardColor[i, j] = -4;
            if (matrix[i, j, 2] == 11) boardColor[i, j] = -11;
            if (matrix[i, j, 2] == 12) boardColor[i, j] = -12;
            boundColor++;
            if ((matrix[i, j, 3] == 1f && matrix[i, j, 6] == 0f) || (matrix[i, j, 3] == 0f && matrix[i, j, 6] == 1f))
            {
                if (boardColor[i - 1, j] == 0)
                {
                    dfs(i - 1, j);
                }
                else
                if (boardColor[i + 1, j] == 0)
                {
                    dfs(i + 1, j);
                }
            }
            else
            if ((matrix[i, j, 3] == 0.7071068f && matrix[i, j, 6] == 0.7071068f) || (matrix[i, j, 3] == -0.7071068f && matrix[i, j, 6] == 0.7071068f))
            {
                if (boardColor[i, j - 1] == 0)
                {
                    dfs(i, j - 1);
                }
                else
                if (boardColor[i, j + 1] == 0)
                {
                    dfs(i, j + 1);
                }
            }
        }
        else
        if (matrix[i, j, 2] == 2) { boardColor[i, j] = -9; }
        else
            if (matrix[i, j, 2] != -1)
        {
            dfsWire(i, j, boundColor);
        }

    }

    private double maxPotential = -1;
    void Save()
    {

        StreamWriter dataWriter = new StreamWriter(playerDataPath);
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    for (int j2 = 0; j2 < 9; j2++)
                        dataWriter.Write(matrix[i, j, j2] + " ");
                    dataWriter.WriteLine();
                }
            }
        {
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    dataWriter.Write(boardColor[i, j] + " ");
                }
                dataWriter.WriteLine();
            }
            Gauss.ComplexDouble allResist;

            for (int currentcolor = 0; currentcolor < countColor; currentcolor++)
            {
                allResist = (Gauss.ComplexDouble)0.0;
                for (int i = 0; i < 14; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (boardColor[i, j] < 0 && matrix[i, j, 2] != 3 && matrix[i, j, 2] != 2 && matrix[i, j, 2] != 1)
                        {
                            if (resistorsMatrixArr[i, j].firstUzel == colorArray[currentcolor])
                            {
                                a_matrix[currentcolor, ColorInArray(resistorsMatrixArr[i, j].secondUzel)] += (Gauss.ComplexDouble)(1 / resistorsMatrixArr[i, j].resistorNum);
                                allResist += (Gauss.ComplexDouble)(1 / resistorsMatrixArr[i, j].resistorNum);
                            }
                            if (resistorsMatrixArr[i, j].secondUzel == colorArray[currentcolor])
                            {
                                a_matrix[currentcolor, ColorInArray(resistorsMatrixArr[i, j].firstUzel)] += (Gauss.ComplexDouble)(1 / resistorsMatrixArr[i, j].resistorNum);
                                allResist += (Gauss.ComplexDouble)(1 / resistorsMatrixArr[i, j].resistorNum);
                            }
                        }
                    }
                }
                a_matrix[currentcolor, currentcolor] = (Gauss.ComplexDouble)(-allResist);
            }
            for (int i = 0; i < countColor; i++)
            {
                a_matrix[ColorInArray(resistorsMatrixArr[xPosEds, yPosEds].firstUzel), i] = (Gauss.ComplexDouble)0.0;
                a_matrix[ColorInArray(resistorsMatrixArr[xPosEds, yPosEds].secondUzel), i] = (Gauss.ComplexDouble)0.0;
            }
            a_matrix[ColorInArray(resistorsMatrixArr[xPosEds, yPosEds].firstUzel), ColorInArray(resistorsMatrixArr[xPosEds, yPosEds].firstUzel)] = (Gauss.ComplexDouble)1.0;
            a_matrix[ColorInArray(resistorsMatrixArr[xPosEds, yPosEds].secondUzel), ColorInArray(resistorsMatrixArr[xPosEds, yPosEds].secondUzel)] = (Gauss.ComplexDouble)1.0;
            b_vector[ColorInArray(resistorsMatrixArr[xPosEds, yPosEds].secondUzel)] = (Gauss.ComplexDouble)resistorsMatrixArr[xPosEds, yPosEds].resistorNum;

            Gauss.LinearSystem U = new Gauss.LinearSystem(a_matrix, b_vector, 0.000000001);

            Gauss.ComplexDouble[] result1 = U.XVector;


            for (int i = 0; i < countColor; i++)
            {
                result[i] = result1[i].Abs;
                dataWriter.Write(result[i] + " ");
                if (result[i]>maxPotential)
                    { maxPotential = result[i]; }
            }
            dataWriter.WriteLine();
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (ColorInArray(boardColor[i, j]) != 50)
                    {
                        answerMatrix[i,j] = result[ColorInArray(boardColor[i, j])];
                        if (answerMatrix[i, j] != 0 && maxPotential != 0)
                        {
                            potentialSprite.transform.localScale = new Vector3(1f, (float)(answerMatrix[i, j] / maxPotential), 1f);
                            Instantiate(potentialSprite, new Vector3(i - 0.5f, j - 0.5f, 0f), Quaternion.Euler(0, 0, 0));
                            potentialSprite.transform.localScale = new Vector3(1f, (float)(maxPotential / answerMatrix[i, j]), 1f);
                        } else
                        {
                            potentialSprite.transform.localScale = new Vector3(1f, (float)(0.1), 1f);
                            Instantiate(potentialSprite, new Vector3(i - 0.5f, j - 0.5f, 0f), Quaternion.Euler(0, 0, 0));
                            potentialSprite.transform.localScale = new Vector3(1f, (float)(10), 1f);
                        }


                        dataWriter.Write(result[ColorInArray(boardColor[i, j])] + " ");
                    }
                    else
                    {
                        answerMatrix[i, j] = -1;
                        dataWriter.Write(-1 + " ");
                    }
                }
                dataWriter.WriteLine();
            }

            for (int x = 0; x < 14; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    if (matrix[x, y, 2] == 6)
                    {

                        if ((matrix[x, y, 3] == 1f && matrix[x, y, 6] == 0f) || (matrix[x, y, 3] == 0f && matrix[x, y, 6] == 1f))
                        {

                            if (answerMatrix[x - 1, y] < answerMatrix[x + 1, y])
                            {
                                answerMatrix[x, y] = Math.Abs(answerMatrix[x - 1, y] - answerMatrix[x + 1, y]) / matrix[x, y, 7];
                                Instantiate(arrowLeft, new Vector3(x, y, 0f), Quaternion.Euler(0, 0, 0));
                            }
                            else
                            if (answerMatrix[x - 1, y] > answerMatrix[x + 1, y])
                            {
                                answerMatrix[x, y] = Math.Abs(answerMatrix[x - 1, y] - answerMatrix[x + 1, y]) / matrix[x, y, 7];
                                Instantiate(arrowRight, new Vector3(x, y, 0f), Quaternion.Euler(0, 0, 0));
                            }
                        }
                        if ((matrix[x, y, 3] == 0.7071068f && matrix[x, y, 6] == 0.7071068f) || (matrix[x, y, 3] == -0.7071068f && matrix[x, y, 6] == 0.7071068f))
                        {
                            if (answerMatrix[x, y - 1] < answerMatrix[x, y + 1])
                            {
                                answerMatrix[x, y] = Math.Abs(answerMatrix[x, y - 1] - answerMatrix[x, y + 1]) / matrix[x, y, 7];
                                Instantiate(arrowDown, new Vector3(x, y, 0f), Quaternion.Euler(0, 0, 0));
                            }
                            else
                            if (answerMatrix[x, y - 1] > answerMatrix[x, y + 1])
                            {
                                Instantiate(arrowUp, new Vector3(x, y, 0f), Quaternion.Euler(0, 0, 0));
                                answerMatrix[x, y] = Math.Abs(answerMatrix[x, y - 1] - answerMatrix[x, y + 1]) / matrix[x, y, 7];
                            }
                        }
                    }
                }
            }

            for (int x = 0; x < 14; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    if (matrix[x, y, 2] == 3 && ((matrix[x, y, 3] == 1f && matrix[x, y, 6] == 0f) || (matrix[x, y, 3] == 0f && matrix[x, y, 6] == 1f)) && answerMatrix[x + 1, y] == answerMatrix[x - 1, y])
                    {
                        shortCircuit = true;
                    } else
                    if (matrix[x, y, 2] == 3 && ((matrix[x, y, 3] == 0.7071068f && matrix[x, y, 6] == 0.7071068f) || (matrix[x, y, 3] == -0.7071068f && matrix[x, y, 6] == 0.7071068f)) && answerMatrix[x, y + 1] == answerMatrix[x, y - 1])
                    {
                        shortCircuit = true;
                    } else
                    if (matrix[x, y, 2] == 1 || matrix[x, y, 2] == 3)
                    {
                        amperage = 0;
                        Initialized();
                        Amperage(x, y);
                        answerMatrix[x, y] = amperage;
                    }

                }
            }

            for (int i = 0; i < countColor; i++)
            {
                for (int j = 0; j < countColor; j++)
                {
                    dataWriter.Write(a_matrix[i,j] + " ");
                }
                dataWriter.WriteLine();
            }


            /*for (int i = 0; i < countColor; i++)
            {
                for (int j = 0; j < countColor; j++)
                {
                    dataWriter.Write(a_matrix[i,j]+ " ");
                }
                dataWriter.WriteLine(b_vector[i]);
            }
                    for (int i = 0; i < 14; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            if (boardColor[i,j] < 0)
                            {
                                dataWriter.WriteLine(resistorsMatrixArr[i,j].firstUzel + " " + resistorsMatrixArr[i, j].secondUzel + " " + i + " " + j + " " + resistorsMatrixArr[i,j].resistorNum);
                            }
                        }            
                    }*/

            dataWriter.Flush();
            dataWriter.Close();
        }
    }

    int ColorInArray(int color)
    {
        for (int i = 0; i < countColor; i++)
        {
            if (colorArray[i] == color) return i;
        }
        return 50;
    }

    public static void Load()
    {
        StreamReader dataReader = new StreamReader(playerDataPath);
        string[] stringdata = new string[9];
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                stringdata = dataReader.ReadLine().Split(' ');
                for (int a = 0; a < 9; a++)
                {
                    matrix[i, j, a] = float.Parse(stringdata[a]);
                }
                // Debug.Log(matrix[i, j, 0] + " " + matrix[i, j, 1] + " " + matrix[i, j, 2]);
            }
        }
        dataReader.Close();
    }


    public static bool doSaveWindow, doLoadWindow, info, shortCircuit;
    private Rect windowRect0 = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, Screen.height / 3);
    double potential = -1;

    void OnGUI()
    {
        if (shortCircuit)
        {
            GUI.color = Color.cyan;
            windowRect0 = GUI.Window(2, windowRect0, DoMyWindow, "Произошло короткое замыкание!");
        }
        if (info)
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(Screen.width / 45, 5.2f * Screen.height / 6, Screen.width / 7, Screen.height / 10), "Общая сила тока равна " + answerMatrix[xPosEds, yPosEds].ToString("0.000") + " A");
            GUI.Label(new Rect(Screen.width / 45, 5.35f * Screen.height / 6, Screen.width / 7, Screen.height / 10), "Общее" +
                " сопротивление равно " + (matrix[xPosEds, yPosEds,8] / answerMatrix[xPosEds, yPosEds]).ToString("0.000") + " Ом");

        }

        if (potential != -1)
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(Screen.width / 45, 5.6f * Screen.height / 6, Screen.width / 7, Screen.height / 10), "Потенциал в данной точке равен " + potential.ToString("0.000") + " В");
        }

        if (electricity != -1)
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(Screen.width / 45, 5.6f * Screen.height / 6, Screen.width / 7, Screen.height / 10), "Сила тока в данном элементе " + electricity.ToString("0.000") + " A");
        }

        if (potentialOnVolt != -1)
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(Screen.width / 45, 5.6f * Screen.height / 6, Screen.width / 7, Screen.height / 10), "Напряжение равно " + potentialOnVolt.ToString("0.000") + " В");
        }

        if (amperage != 0)
        {
            GUI.color = Color.cyan;
            GUI.Label(new Rect(Screen.width / 45, 5.6f * Screen.height / 6, Screen.width / 7, Screen.height / 10), "Сила тока равна " + amperage.ToString("0.000") + " A");
        }

        if (doSaveWindow)
        {
            GUI.color = Color.cyan;
            windowRect0 = GUI.Window(1, windowRect0, DoMyWindow, "Введите имя файла для сохранения:");
        }

        if (doLoadWindow)
        {
            GUI.color = Color.cyan;
            windowRect0 = GUI.Window(0, windowRect0, DoMyWindow, "Введите имя файла для загрузки:");
        }
    }

    void DoMyWindow(int windowID)
    {

        if (windowID == 2)
        {
            GUI.color = Color.cyan;
            if (GUI.Button(new Rect(Screen.width / 9, Screen.height / 4, Screen.width / 9, Screen.width / 50), "Подтвердить"))
            {
                shortCircuit = false;
            }

        }
        else
        {
            playerDataPath = "";
            GUI.color = Color.cyan;
            path = GUI.TextField(new Rect(10, Screen.height / 10, (Screen.width / 3 - 20), Screen.height / 30), path);
            if (GUI.Button(new Rect(Screen.width / 21, Screen.height / 4, Screen.width / 9, Screen.width / 50), "Подтвердить"))
            {

                playerDataPath = "D:/temp.db";

                GUI.color = Color.cyan;


                GUI.color = Color.cyan;
                if (windowID == 0)
                {
                    Initialized();
                    Load();
                    for (int i = 0; i < 14; i++)
                    {
                        for (int j = 0; j < 11; j++)
                        {
                            RaycastHit2D hit = Physics2D.Raycast(new Vector3(matrix[i, j, 0], matrix[i, j, 1], -10f), Vector2.zero);
                            if ((int)matrix[i, j, 2] != -1)
                            {
                                hit.transform.GetComponent<SpriteRenderer>().sprite = arraySprites[(int)matrix[i, j, 2]];
                            }
                            else
                                hit.transform.GetComponent<SpriteRenderer>().sprite = null;
                            hit.transform.rotation = new Quaternion(matrix[i, j, 4], matrix[i, j, 5], matrix[i, j, 6], matrix[i, j, 3]);
                            hit.collider.gameObject.GetComponent<BlockScript>().resist = (int)matrix[i, j, 7];
                            hit.collider.gameObject.GetComponent<BlockScript>().eds = (int)matrix[i, j, 8];

                        }
                    }
                    Debug.Log("Загрузка произведена, мать твою за ногу");
                }
                doLoadWindow = false;


                if (windowID == 1)
                {
                    Start();
                    int i = 1;
                    int xcounter = 0, ycounter = 0;
                    for (int x = 0; x < 14; x++)
                    {
                        for (int y = 0; y < 11; y++)
                        {
                            int spriteInt = -1;
                            RaycastHit2D hit = Physics2D.Raycast(new Vector3(x, y, -10f), Vector2.zero);
                            spriteInt = getIntOfImage(hit.transform.GetComponent<SpriteRenderer>().sprite);
                            Saves[i, 0] = x;
                            Saves[i, 1] = y;
                            Saves[i, 2] = spriteInt;
                            Saves[i, 3] = hit.transform.rotation.w;
                            Saves[i, 4] = hit.transform.rotation.x;
                            Saves[i, 5] = hit.transform.rotation.y;
                            Saves[i, 6] = hit.transform.rotation.z;
                            Saves[i, 7] = hit.collider.gameObject.GetComponent<BlockScript>().resist;
                            Saves[i, 8] = hit.collider.gameObject.GetComponent<BlockScript>().eds;
                            if (ycounter > 10)
                            {
                                ycounter = 0;
                                xcounter++;
                            }
                            for (int j = 0; j < 9; j++)
                                matrix[xcounter, ycounter, j] = Saves[i, j];
                            i++;
                            ycounter++;
                        }

                    }
                    for (int x = 0; x < 14; x++)
                    {
                        for (int y = 0; y < 11; y++)
                        {
                            boardColor[x, y] = 0;
                        }
                    }
                    boundColor = 0;


                    for (int x = 0; x < 14; x++)
                    {
                        for (int y = 0; y < 11; y++)
                        {
                            if (matrix[x, y, 2] == 3)
                            {
                                xPosEds = x;
                                yPosEds = y;
                                dfs(x, y);
                            }
                        }
                    }

                    for (int x = 0; x < 14; x++)
                    {
                        for (int y = 0; y < 11; y++)
                        {
                            if (matrix[x, y, 2] == 6 || matrix[x, y, 2] == 3 || matrix[x, y, 2] == 1 || matrix[x, y, 2] == 2)
                            {
                                resistorsMatrixArr[x, y].resistorNum = matrix[x, y, 7];
                                if ((matrix[x, y, 3] == 1 && matrix[x, y, 6] == 0) || (matrix[x, y, 3] == 0 && matrix[x, y, 6] == 1))
                                {
                                    //Debug.Log(boardColor[x + 1, y]);
                                    if (x > 0)
                                    {
                                        if (boardColor[x - 1, y] > 0)
                                            if (resistorsMatrixArr[x, y].firstUzel == 0)
                                                resistorsMatrixArr[x, y].firstUzel = boardColor[x - 1, y];
                                            else
                                                if (resistorsMatrixArr[x, y].secondUzel == 0)
                                                resistorsMatrixArr[x, y].secondUzel = boardColor[x - 1, y];
                                    }
                                    if (x < 13)
                                    {
                                        if (boardColor[x + 1, y] > 0)
                                            if (resistorsMatrixArr[x, y].firstUzel == 0)
                                                resistorsMatrixArr[x, y].firstUzel = boardColor[x + 1, y];
                                            else
                                            if (resistorsMatrixArr[x, y].secondUzel == 0)
                                                resistorsMatrixArr[x, y].secondUzel = boardColor[x + 1, y];
                                    }
                                }
                                else
                                {
                                    if (y > 0)
                                    {
                                        if (boardColor[x, y - 1] > 0)
                                            if (resistorsMatrixArr[x, y].firstUzel == 0)
                                                resistorsMatrixArr[x, y].firstUzel = boardColor[x, y - 1];
                                            else
                                            if (resistorsMatrixArr[x, y].secondUzel == 0)
                                                resistorsMatrixArr[x, y].secondUzel = boardColor[x, y - 1];
                                    }
                                    if (x < 10)
                                    {
                                        if (boardColor[x, y + 1] > 0)
                                            if (resistorsMatrixArr[x, y].firstUzel == 0)
                                                resistorsMatrixArr[x, y].firstUzel = boardColor[x, y + 1];
                                            else
                                                if (resistorsMatrixArr[x, y].secondUzel == 0)
                                                resistorsMatrixArr[x, y].secondUzel = boardColor[x, y + 1];
                                    }
                                }
                            }
                            if (matrix[x, y, 2] == 3)
                            {
                                resistorsMatrixArr[x, y].resistorNum = matrix[x, y, 8];
                            }
                        }
                    }

                    bool f;
                    int length = 0;

                    for (int x = 0; x < 14; x++)
                    {
                        for (int y = 0; y < 11; y++)
                        {
                            f = true;
                            for (int k = 0; k < length; k++)
                            {
                                if (colorArray[k] == boardColor[x, y])
                                {
                                    f = false;
                                    break;
                                }
                            }
                            if (f && boardColor[x, y] > 0)
                            {
                                colorArray[length] = boardColor[x, y];
                                length++;
                                countColor++;
                            }
                        }
                    }
                    Save();
                    Debug.Log("saved, блять, а теперь иди нахуй");
                    info = true;
                    countColor = 0;
                    doSaveWindow = false;
                }
            }

            if (GUI.Button(new Rect(Screen.width / 6, Screen.height / 4, Screen.width / 9, Screen.width / 50), "Отмена"))
            {
                doSaveWindow = false;
                doLoadWindow = false;
            }
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        
    }

    private void Start()
    {
        for (int i = 0; i < 14; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                answerMatrix[i, j] = -1;
                boardColor[i, j] = -1;
                a_matrix[i, j] = (Gauss.ComplexDouble)0.0;
            }
            b_vector[i] = (Gauss.ComplexDouble)0.0;
        }
    }
    public double potentialOnVolt;
    public Camera _camera;
    void Update()
    {
        doAnalizeWindow = false;
        if (_camera.ScreenToWorldPoint(Input.mousePosition).x >-0.5 && _camera.ScreenToWorldPoint(Input.mousePosition).x  < 13.5 && _camera.ScreenToWorldPoint(Input.mousePosition).y > 0 && _camera.ScreenToWorldPoint(Input.mousePosition).y < 11)
        {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (answerMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y] != -1 && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 2] != 6 && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 2] != 4 && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 2] != 1 && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 2] != 2)
                potential = answerMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y];
            else
                potential = -1;

            if (matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 2] == 6)
            {
                electricity = answerMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y];

            }
            else electricity = -1;

            if (matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 2] == 2)
            {
                if ((matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 3] == 1f && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 6] == 0f) || (matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 3] == 0f && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 6] == 1f))
                {
                    potentialOnVolt = Math.Abs(answerMatrix[(int)hit.transform.position.x + 1, (int)hit.transform.position.y] - answerMatrix[(int)hit.transform.position.x - 1, (int)hit.transform.position.y]);
                }
                if ((matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 3] == 0.7071068f && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 6] == 0.7071068f) || (matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 3] == -0.7071068f && matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 6] == 0.7071068f))
                {
                    potentialOnVolt = Math.Abs(answerMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y - 1] - answerMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y - 1]);
                }
            }
            else potentialOnVolt = -1;

            if (matrix[(int)hit.transform.position.x, (int)hit.transform.position.y, 2] == 1)
            {
                amperage = answerMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y];
            }
            else amperage = 0;
        }

        if (Input.GetKeyDown("s"))
        {
            doSaveWindow = true;
        }

        if (Input.GetKeyDown("l"))
        {
            doLoadWindow = true;
        }
    }
}




