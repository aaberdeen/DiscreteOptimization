using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Timers;

namespace knapsack_solution
{
    public partial class Form1 : Form
    {
        //public static void Main(string[] args);
        static System.Timers.Timer _timer;
        static int[] takenArray;
        public Item[] itemArray;
        private Item[] sortedByWeightList;
        private Item[] sortedByWeightRevList;
        private Item[] sortedByVToWRatioList;
        private Item[] sortedByValueList;
        public static bool stop;
        private List<TreeNodeStruct> nodeList;

        struct TreeNodeStruct
        {
            public UInt16 _level;
            public Int32 value;
            public Int32 weight;
            public Int32 estimate;
        }

        public Form1()
        {
            InitializeComponent();
            Start();

            string[] args = Environment.GetCommandLineArgs();
            string lines = args[0];
            string line2 = args[1];


            IP_textBox.AppendText(lines + " \n");
            if (line2 != null)
            {
                IP_textBox.AppendText(line2 + " \n");
            }
            solve(line2 + " ");

           // solve(@"C:\Users\andy\Google Drive\Discrete Optimization\knapsack\data\ks_1000_0");
           //dynamicSolve();
        }

        static void Start()
        {

            _timer = new System.Timers.Timer(3000); // Set up the timer for 3 seconds
            //
            // Type "_timer.Elapsed += " and press tab twice.
            //
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; // Enable it
        }
        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            stop = true;
        }

        private void Solve_button_Click(object sender, EventArgs e)
        {
            OP_textBox.Clear();
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /**
     * Read the instance, solve it, and print the solution in the standard output
     */
        public void solve(string fileName)
        {
            // read the lines out of the file


            List<String> lines = new List<String>();

            // BufferedReader input =  new BufferedReader(new FileReader(fileName));
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line); // Add to list.
                    IP_textBox.AppendText(line+" \n");
                }
            }
            //try {
            //    //String line = null;
            //    foreach(string line in IP_textBox.Lines)
            //    {
            //        lines.Add(line);
            //    }
            //}
            //finally 
            //{
            //}

            // parse the data in the file
            String[] firstLine = lines[0].Split(' ');
            int items = Convert.ToInt32(firstLine[0]);
            int capacity = Convert.ToInt32(firstLine[1]);

            //Item[] itemArray = new Item[items];
            itemArray = new Item[items];

            for (int i = 1; i < items + 1; i++)
            {
                String line = lines[i];
                String[] parts = line.Split(' ');

                itemArray[i - 1] = new Item((i - 1), Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]));
            }



            

            //var opList = greedySorted(items, capacity, itemArray);
           // var opList = dynamicSolve(items, capacity, itemArray);
            TreeNode bestNode = relaxBranchAndBound(items, capacity);
          

            //string output = "";
            //for (int i = 0; i < items; i++)
            //{
            //    OP_textBox.AppendText(opList[i].taken + " ");
            //    output += opList[i].taken + " ";

            //}
            string output = "";
            output += bestNode.value + " "+ 0+ " \n";
            OP_textBox.AppendText(bestNode.value.ToString() +" " + 0 +" \n");
            for (int i = 0; i < items; i++)
            {
                OP_textBox.AppendText(takenArray[i] + " ");
                output += takenArray[i] + " ";

            }
            System.Diagnostics.Debug.WriteLine(output);
            Console.Out.WriteLine(output);

        }

        private Item[] greedySorted(int items, int capacity, Item[] itemArray)
        {
            //sort the array
            var sortedList = itemArray.OrderBy(i => i.value).ToArray();
            int value = 0;
            int weight = 0;
            for (int i = 0; i < items; i++)
            {
                if (weight + sortedList[i].weight <= capacity)
                {
                    sortedList[i].taken = 1;
                    value += sortedList[i].value;
                    weight += sortedList[i].weight;
                }
                else
                {
                    sortedList[i].taken = 0;
                }
            }

            OP_textBox.AppendText(value + " 0" + "\n");
            System.Diagnostics.Debug.WriteLine(value + " 0");
            Console.Out.WriteLine(value + " 0");
            var opList = itemArray.OrderBy(i => i._index).ToArray();
            return opList;
        }

        private Item[] dynamicSolve(int items, int capacity, Item[] itemArray)
        {
           // int capacity = 11;
            //int items = 4;
            //Item[] itemArray = new Item[4];
            //itemArray[0] = new Item(0, 8, 4);
            //itemArray[1] = new Item(1, 10, 5);
            //itemArray[2] = new Item(2, 15, 8);
            //itemArray[3] = new Item(3, 4, 3);

            Item[,] table = new Item[capacity, items];

            for (int i = 0; i < items; i++)
            {
                for (int k = 0; k < capacity; k++)
                {
                    int spairCapacity = k - itemArray[i].weight;
                    if (spairCapacity >= 0)  // room to fit this item in bag
                    {
                        int rowToLeftValue = 0;
                        int valueIfChosen = 0;
                        int weightIfChosen = 0;
                        if (i - 1 >= 0)
                        {
                            rowToLeftValue = table[k, i - 1].value;
                        }


                        if (spairCapacity > 0)
                        {
                            if (i - 1 >= 0)
                            {
                                valueIfChosen = itemArray[i].value + table[spairCapacity, i - 1].value;
                                weightIfChosen = itemArray[i].weight + table[spairCapacity, i - 1].weight;
                            }
                            else
                            {
                                valueIfChosen = itemArray[i].value;
                                weightIfChosen = itemArray[i].weight;
                            }

                        }
                        else
                        {
                            valueIfChosen = itemArray[i].value;
                            weightIfChosen = itemArray[i].weight;
                        }
                        if (rowToLeftValue < valueIfChosen)  //choose to take
                        {
                            table[k, i] = new Item(itemArray[i]._index, valueIfChosen, weightIfChosen);
                            //if (spairCapacity > 0)
                            //{
                            //    //add the value from the previous columb with that capacity
                            //    //int leftColumbSpairIndex = 0;
                            //    if (i - 1 >= 0)
                            //    {
                            //        table[k, i].weight = table[k, i].weight + table[spairCapacity, i - 1].weight;
                            //        table[k, i].value = table[k, i].value + table[spairCapacity, i - 1].value;
                            //    }
                            //}
                        }
                        else
                        {
                            if (i - 1 >= 0)
                            {
                                table[k, i] = new Item(table[k, i - 1]._index, table[k, i - 1].value, table[k, i - 1].weight);
                            }
                            

                        }
                    }
                    else
                    {
                        int rowToLeftWeight = 0;
                        if (i - 1 >= 0)
                        {
                            rowToLeftWeight = table[k, i - 1].weight;
                        }
                        spairCapacity = k - rowToLeftWeight;
                        if (rowToLeftWeight > 0)  // stick it in
                        {
                            table[k, i] = new Item(table[k, i - 1]._index, table[k, i - 1].value, table[k, i - 1].weight);
                        }
                        else
                        {
                            table[k, i] = new Item(0, 0, 0);
                        }
                    }

                }
            }

            //debug op table
            //for (int k = 1; k < capacity; k++)
            //{
            //    string line = null;
            //    for (int i = 0; i < items; i++)
            //    {
            //        line += table[k, i].value.ToString() + " ";

            //    }
            //    // DebugTextBox.AppendText(line);
            //    System.Diagnostics.Debug.WriteLine(line);
            //}

            int[] taken = new int[items];
            //  for(int k = bagKapacity-1; k>=0; k--)
            //  {
            int bk = capacity-1;
            int _numberOfItems = items;
            
                for (int i = _numberOfItems - 1; i >= 0; i--)
                {
                    int cellValue = 0;
                    int cellToLeftValue = 0;
                    cellValue = table[bk, i].value;
                    if (i > 0)
                    {
                        cellToLeftValue = table[bk, i - 1].value;
                    }

                    if (cellValue == cellToLeftValue)
                    {//item not taken
                        taken[i] = 0;
                        itemArray[i].taken = 0;
                    }
                    else
                    {//item taken
                        taken[i] = 1;
                        itemArray[i].taken = 1;
                        //remove the weight of that item
                        bk = bk - itemArray[i].weight;
                    
                    }
                }
                int value = table[capacity-1, items-1].value;

                OP_textBox.AppendText(value + " 0" + "\n");
                System.Diagnostics.Debug.WriteLine(value + " 0");
                Console.Out.WriteLine(value + " 0");
                return itemArray;
        }

        private TreeNode bestNode;
        private int _items;


        private void pruneRecursive(TreeNodeStruct node)
        {
            if (stop == true)
            {
                return;
            }
            else
            {
                if (node._level == _items) // we are at the bottom of the tree
                {
                    if ((node.weight >= 0) & (node.value >= bestNode.value))// & (node.estimate >= bestNode.estimate))
                    {
                        bestNode.value = node.value;
                        bestNode.weight = node.weight;
                        bestNode._level = node._level;
                        bestNode.estimate = node.estimate;
                        foreach (Item n in sortedByWeightRevList)
                        {
                            if (n.taken == 0)
                            {
                                takenArray[n._index] = 0;
                            }
                            else
                            {
                                takenArray[n._index] = 1;
                            }

                        }

                        //  bestNode.chosen = node.chosen;
                    }
                    else
                    {//not best value no need to record
                       // node = null;
                    }

                }
                else // we are still working our way down
                {
                    if ((node.weight >= 0) )//& (node.estimate >= bestNode.estimate))
                    {

                        //take item [1]
                        //node.contains[1] = new TreeNode((Int16)(node._level + 1), (Int16)(node.value + sortedByWeightRevList[node._level].value), (Int16)(node.weight - sortedByWeightRevList[node._level].weight), node.estimate);
                        //node.contains = new TreeNode((Int16)(node._level + 1), (Int16)(node.value + sortedByWeightRevList[node._level].value), (Int16)(node.weight - sortedByWeightRevList[node._level].weight), node.estimate);
                        
                        TreeNodeStruct Contains;
                        Contains._level = (UInt16)(node._level + 1);
                        Contains.value = (Int32)(node.value + sortedByWeightRevList[node._level].value);
                        Contains.weight = (Int32)(node.weight - sortedByWeightRevList[node._level].weight);
                        Contains.estimate = node.estimate;
                        
                        
                        
                        //for (int i = 0; i < _items; i++)
                        //{
                        //    node.contains[1].chosen[i] = node.chosen[i];
                        //}
                        sortedByWeightRevList[node._level].taken = 1;
                        //  node.contains[1].chosen[node._level] = true;
                        //pruneRecursive(node.contains[1]);
                        pruneRecursive(Contains);


                        //memory clean up whenever we branch right (don't take we can null the branch left) if there was a best value we should have stored it.
                        //node.contains[1] = null;
                       // node.contains = null;

                        //don't take item [0]
                        //node.contains[0] = new TreeNode((Int16)(node._level + 1), node.value, node.weight, (Int16)(node.estimate - sortedByWeightRevList[node._level].value));
                        //node.contains = new TreeNode((Int16)(node._level + 1), node.value, node.weight, (Int16)(node.estimate - sortedByWeightRevList[node._level].value));

                        Contains._level = (UInt16)(node._level + 1);
                        Contains.value = node.value;
                        Contains.weight = node.weight;
                        Contains.estimate = (Int32)(node.estimate - sortedByWeightRevList[node._level].value);
                        
                        //for (int i = 0; i < _items; i++)
                        //{
                        //    node.contains[0].chosen[i] = node.chosen[i];
                        //}
                        sortedByWeightRevList[node._level].taken = 0;
                        //    node.contains[0].chosen[node._level] = false;
                         if (node._level > 9905)
                        {
                        }

                        
                        //pruneRecursive(node.contains[0]);
                         pruneRecursive(Contains);
                       

                    }
                    else
                    {
                        //  node = null;
                    }
                    //node.contains[1] = 
                }
            }
        }

        private TreeNode relaxBranchAndBound(int items, int capacity)
        {
            // sort items by v/w largest first
            //sort the array
            sortedByVToWRatioList = itemArray.OrderBy(i => i.valToWeightRatio).ToArray();
            sortedByWeightRevList = itemArray.OrderBy(i => i.valToWeightRatio).Reverse().ToArray();
            sortedByValueList = itemArray.OrderBy(i => i.value).Reverse().ToArray();
            sortedByWeightList = itemArray.OrderBy(i => i.weight).Reverse().ToArray();
            int optWeight = 0;
            int optValue = 0;
            int oversized = 0;
            // add values untill weight is exceeded
            // then add a fraction of the one that exceeded it.
            // summ up the values of these 
            // this is your optimistic evaluation.
            for (int i = items-1; i >= 0; i--)
            {
                    if (sortedByVToWRatioList[i].weight > capacity) // will this item fit in the bag on it's own?
                    {// no well we will include it in the estemate. this is a frig we should remove from the list and mark as not fitted realy
                        oversized += sortedByVToWRatioList[i].weight;
                    }
                
                    if ((optWeight + sortedByVToWRatioList[i].weight) <= capacity)
                    {
                        optValue =optValue+ sortedByVToWRatioList[i].value;
                        optWeight = optWeight+ sortedByVToWRatioList[i].weight;
                    }
                    else
                    {
                        int remainder = capacity - optWeight;
                        optValue = optValue+((int)(((float)remainder / (float)sortedByVToWRatioList[i].weight) * (float)sortedByVToWRatioList[i].value));
                        optValue += oversized;
                        i = 0;
                    }
                
            }   

            //then using unsorted list
            //start  value = 0 room = bag size estimate = above calculated value



           // Item[] table = new Item[items];


            TreeNode start = new TreeNode(0, 0, (Int16)capacity, (Int16)(optValue * 750));// (100000000 - 84602860-12439-290-23070-450-390-29410-77-154));// optValue*5);  		node.estimate	84602860	int
            TreeNodeStruct startStruct;
            startStruct._level = 0;
            startStruct.value = 0;
            startStruct.weight = (Int32)capacity;
            startStruct.estimate = (Int32)(optValue);

            _items = items;
             bestNode = new TreeNode(0,0,0,0);
             takenArray = new int[items];
             nodeList = new List<TreeNodeStruct>();
             pruneRecursive(startStruct);

                
                
              




           

            //int[] taken = new int[items];
            ////  for(int k = bagKapacity-1; k>=0; k--)
            ////  {
            //int bk = capacity - 1;
            //int _numberOfItems = items;

            //for (int i = _numberOfItems - 1; i >= 0; i--)
            //{
            //    int cellValue = 0;
            //    int cellToLeftValue = 0;
            //    cellValue = table[bk, i].value;
            //    if (i > 0)
            //    {
            //        cellToLeftValue = table[bk, i - 1].value;
            //    }

            //    if (cellValue == cellToLeftValue)
            //    {//item not taken
            //        taken[i] = 0;
            //        itemArray[i].taken = 0;
            //    }
            //    else
            //    {//item taken
            //        taken[i] = 1;
            //        itemArray[i].taken = 1;
            //        //remove the weight of that item
            //        bk = bk - itemArray[i].weight;

            //    }
            //}
            //int value = table[capacity - 1, items - 1].value;

            //OP_textBox.AppendText(value + " 0" + "\n");
            //System.Diagnostics.Debug.WriteLine(value + " 0");
            //Console.Out.WriteLine(value + " 0");
             return bestNode;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }





    }

    public partial class Item
    {
        public int _index;
        public int value;
        public int weight;
        public float valToWeightRatio;
        public int taken;
        public int _estimate;

        public Item(int index, int v, int w)
        {
            _index = index;
            value = v;
            weight = w;
            if (v > 0)
            {
                valToWeightRatio = (float)v / (float)w;
            }
            taken = 0;
        }

        public Item(int index, int v, int w, int estimate)
        {
            _index = index;
            value = v;
            weight = w;
            _estimate = estimate;
            if (v > 0)
            {
                valToWeightRatio = (float)v / (float)w;
            }
            taken = 0;
        }

    }
    public partial class TreeNode
    {
        public UInt16 _level;
        public Int32 value;
        public int weight;
        public int estimate;
        //public TreeNode[] contains = new TreeNode[2];
        public TreeNode contains;
       // public bool[] chosen;

        public TreeNode(UInt16 level, Int32 v, Int32 w, Int32 e)
        {
            value = v;
            weight = w;
            estimate = e;
            _level = level;
          //  chosen = new bool[items];
        }

    }


}
