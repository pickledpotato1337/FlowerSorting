using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    class Program
    {
        //this goes to the finishing product
        public class Valueslist
        {
            public string Name { get; set; }
            public List<double> Values { get; set; }

            public Valueslist(string newName)
            {
                Name = newName;
                Values = new List<double>();
            }
            public double SumDistances() {
                double res=0;
                foreach(double bit in Values)
                {
                    res += bit;
                }
                return res;
            }

        }
        public class Connection
        {
            
            public Descriptor StartPoint { get; set; }
            public Descriptor EndPoint { get; set; }
            public double Distance { get; set; }

            public Connection(Descriptor end, Descriptor start)
            {
                StartPoint = start;
                EndPoint = end;
                double xVal = Math.Pow((start.Xcoor - end.Xcoor), 2);
                double yVal = Math.Pow((start.Ycoor - end.Ycoor),2);
                Distance = Math.Sqrt(xVal+yVal);
            }
        }


        
        public class Garden
        {
            public Flower MinVal { get; set; }
            public Flower MaxVal { get; set; }
            public List<Flower> Flowers { get; set; }
            public Garden(List<Flower> input)
            {
                Flowers = input;
               
                
            }

            public void SetNormRange()
            {   
                double desc1 = Flowers[0].SepalLength;
                double desc2 = Flowers[0].SepalWidth;
                double desc3 = Flowers[0].PetalLength;
                double desc4 = Flowers[0].PetalWidth;

                double desc5 = Flowers[0].SepalLength;
                double desc6 = Flowers[0].SepalWidth;
                double desc7 = Flowers[0].PetalLength;
                double desc8 = Flowers[0].PetalWidth;

                foreach (Flower plant in Flowers)
                {
                    if (plant.SepalLength < desc1)
                        desc1 = plant.SepalLength;
                    if (plant.SepalWidth < desc2)
                        desc2 = plant.SepalWidth;
                    if (plant.PetalLength < desc3)
                        desc3 = plant.PetalLength;
                    if (plant.PetalWidth < desc4)
                        desc4 = plant.PetalWidth;

                    if (plant.SepalLength > desc5)
                        desc5 = plant.SepalLength;
                    if (plant.SepalWidth > desc6)
                        desc6 = plant.SepalWidth;
                    if (plant.PetalLength > desc7)
                        desc7 = plant.PetalLength;
                    if (plant.PetalWidth > desc8)
                        desc8 = plant.PetalWidth;
                }

                MinVal = new Flower(desc1, desc2, desc3, desc4, 1); 
                MaxVal = new Flower(desc5, desc6, desc7, desc8, 1);
            }
            public void Normalize()
            {
                SetNormRange();
                //for all the flowers, modify data according to formula: subtract minimum value from each column, then divide by maximum value
                // so  val-minval / maxval-minval
                foreach(Flower weed in Flowers)
                {
                    weed.PetalLength = (weed.PetalLength - MinVal.PetalLength) / (MaxVal.PetalLength - MinVal.PetalLength);
                    weed.PetalWidth = (weed.PetalWidth - MinVal.PetalWidth) / (MaxVal.PetalWidth - MinVal.PetalWidth);
                    weed.SepalLength = (weed.SepalLength - MinVal.SepalLength) / (MaxVal.SepalLength - MinVal.SepalLength);
                    weed.SepalWidth = (weed.SepalWidth - MinVal.SepalWidth) / (MaxVal.SepalWidth - MinVal.SepalWidth);
                }
            }
            public string DecideByNeighbors(Flower someFlower, int neighborCount)
            {
                List<string> neighborValues = this.GetNeighborNames();
                List<Valueslist> neighborDistances = new List<Valueslist>();
                foreach (string element in neighborValues)
                {
                    var initialDistanceList = new Valueslist(element);
                    foreach (Flower plant in Flowers)
                    {
                        ///this needs a bit of a jig, in essence, use the euclidean formula here on all flowers, calculate 
                        ///distance and push it onto the list
                        if (plant.Type.Equals(element))
                        {
                            double desc1 = Math.Pow(plant.SepalLength - someFlower.SepalLength, 2);
                            double desc2 = Math.Pow(plant.SepalWidth - someFlower.SepalWidth, 2);
                            double desc3 = Math.Pow(plant.PetalLength - someFlower.PetalLength, 2);
                            double desc4 = Math.Pow(plant.PetalWidth - someFlower.PetalWidth, 2);
                            double finalDesc = Math.Sqrt(desc1 + desc2 + desc3 + desc4);
                            initialDistanceList.Values.Add(finalDesc);
                        }
                    }
                        initialDistanceList.Values.Sort();
                        var distanceList = new Valueslist(element);
                        for (int i = 0; i < neighborCount; i++)
                        {
                            if (i < initialDistanceList.Values.Count)
                                distanceList.Values.Add(initialDistanceList.Values[i]);
                        }
                        neighborDistances.Add(distanceList);
                    

                }
                double zeroIndex = neighborDistances[0].SumDistances();
                string decicedValue = neighborDistances[0].Name;
                foreach (Valueslist bit in neighborDistances)
                {
                    if (zeroIndex > bit.SumDistances())
                        decicedValue = bit.Name;

                }
                return decicedValue;
            }



            public List<string> GetNeighborNames()
            {
                List<string> initial = new List<string>();
                foreach(Flower plant in Flowers)
                {
                    initial.Add(plant.Type);
                }

                var result = initial.Distinct().ToList();



                return result;
            }
            
            public bool TestKNN(int flowerIndex, int neighborAmount) {

                string testResult = DecideByNeighbors(Flowers[flowerIndex], neighborAmount);
                if (Flowers[flowerIndex].Type.Equals(testResult))
                    return true;


                return false;
            }
            
        }


        public class Flower
        {
            public string Type { get; set; }
            public double SepalLength { get; set; }
            public double SepalWidth { get; set; }
            public double PetalLength { get; set; }
            public double PetalWidth { get; set; }

            public Flower(double sepalLen, double sepalWid, double petalLen, double petalWid, int type)
            {
                SepalLength = sepalLen;
                SepalWidth = sepalWid;
                PetalLength = petalLen;
                PetalWidth = petalWid;
                switch (type)
                {
                    case 1:
                        Type = "Setosa";
                        break;
                    case 2:
                        Type = "Versicolour";
                        break;
                    case 3:
                        Type = "Virginica";
                        break;

                }

            }

           

        }
        public class ReadAndWrite
        {
            public static List<Flower> LoadFlowers(string path)
            {
                List<Flower> result = new List<Flower>();
                string[] readText = File.ReadAllLines(path);
                foreach (string line in readText)
                {
                    string separator="\t";
                    string[] variables = line.Split(separator.ToCharArray());
                    double param1= Double.Parse(variables[0], CultureInfo.InvariantCulture);
                    double param2 = Double.Parse(variables[1], CultureInfo.InvariantCulture);
                    double param3 = Double.Parse(variables[2], CultureInfo.InvariantCulture);
                    double param4 = Double.Parse(variables[3], CultureInfo.InvariantCulture);
                    int param5 = Int32.Parse(variables[4]);
                    var iris = new Flower(param1, param2, param3, param4, param5);
                    result.Add(iris);
                }

                return result;
            }
             
        }

        //here ends final product
        public class Descriptor
        {
            public string Value { get; set; }
            public List<Connection> Neighbors { get; set; }
            public int Xcoor { get; set; }
            public int Ycoor { get; set; }

            public Descriptor(string name, int xVal, int yVal)
            {
                Value = name;
                Xcoor = xVal;
                Ycoor = yVal;
                Neighbors = new List<Connection>();
            }
            public void OverrideDescriptorValue(int neighborQuantity)
            {
                Value = DecideByNeighbors(neighborQuantity);
            }
            
            public void AddNeighbor(Connection newNeighbor)
            {
                Neighbors.Add(newNeighbor);

            }
            public string DecideByNeighbors(int connsPerNeighborClass)
            {   
                List<string>neighborValues= this.FindNeighborNames();
                List<Valueslist> neighborDistances = new List<Valueslist>();
                //for each element find lenghts in neighbours sort, find n shortest
                foreach(string element in neighborValues)
                {
                    var initialDistanceList = new Valueslist(element);
                    foreach(Connection connection in Neighbors)
                    {
                        if (connection.EndPoint.Value.Equals(element))
                        
                            initialDistanceList.Values.Add(connection.Distance);
                        
                    }
                    initialDistanceList.Values.Sort();
                    var distanceList = new Valueslist(element);
                    for(int i=0; i<connsPerNeighborClass; i++)
                    {
                        if(i<initialDistanceList.Values.Count)
                            distanceList.Values.Add(initialDistanceList.Values[i]);
                    }
                    neighborDistances.Add(distanceList);

                    
                }
                double zeroIndex = neighborDistances[0].SumDistances();
                string decicedValue = neighborDistances[0].Name;
                foreach(Valueslist bit in neighborDistances)
                {
                    if (zeroIndex > bit.SumDistances())
                        decicedValue = bit.Name;

                }

                return decicedValue;
            }
            public List<string> FindNeighborNames()
            {
                List<string> initial = new List<string>();
                foreach (Connection elem in Neighbors)
                {
                    initial.Add(elem.EndPoint.Value);
                }
                var result = initial.Distinct().ToList();
                
                    
                
                return result;

            }
        }

        static void Main(string[] args)
        {   //testalgorithm start
            //var d1 = new Descriptor("poor", 2, 2);
            //var d2 = new Descriptor("rich", 1, 1);
            //var d3 = new Descriptor("poor", 3, 3);
            //var d4 = new Descriptor("rich", 1, 2);
            //var d5 = new Descriptor("idk", 1, 1);


            //var c1 = new Connection(d1, d5);
            //var c2 = new Connection(d2, d5);
            //var c3 = new Connection(d3, d5);
            //var c4 = new Connection(d4, d5);
            //d5.Neighbors.Add(c1);
            //d5.Neighbors.Add(c2);
            //d5.Neighbors.Add(c3);
            //d5.Neighbors.Add(c4);
            //string print=d5.DecideByNeighbors(2);
            
            //testAlogithm end

            string filePath = "iris.txt";
            var list = ReadAndWrite.LoadFlowers(filePath);


            Console.WriteLine("specify tested index");
            string index = Console.ReadLine();
            Console.WriteLine("specify neighbor range");
            string count = Console.ReadLine();
            int targetIndex;
            int targetCount;
            if (Int32.TryParse(index, out targetIndex))
                Console.WriteLine("selected index: {0}", targetIndex);
            else
                Console.WriteLine("Input value was not able to be parsed.");
            if (Int32.TryParse(count, out targetCount))
                Console.WriteLine("selected range: {0}", targetCount);
            else
                Console.WriteLine("Input value was not able to be parsed.");

            Garden irises = new Garden(list);
            irises.Normalize(); 
            string print1 = irises.DecideByNeighbors(irises.Flowers[targetIndex],targetCount);
            bool print2 = irises.TestKNN(targetIndex, targetCount);
            Console.WriteLine(print1);
            Console.WriteLine(print2);
            Console.ReadKey();
        }
    }
}
