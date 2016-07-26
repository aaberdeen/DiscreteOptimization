using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coloring
{
    class Program
    { 
        static enum countries
            {belgum,
            denmark,
            france,
            germany,
            netherlands,
            luximburg
            }

        static enum colours
        {
            Black,
            yellow,
            red,
            blue
        }

        static public countries[,] edges = new countries[9, 2];
        static public colours[] countryColours = new colours[6];



        static void Main(string[] args)
        {
            edges[0, 0] = countries.belgum;
            edges[0, 1] = countries.france;

            edges[1, 0] = countries.belgum;
            edges[1, 1] = countries.germany;

            edges[2, 0] = countries.belgum;
            edges[2, 1] = countries.netherlands;

            edges[3, 0] = countries.belgum;
            edges[3, 1] = countries.luximburg;

            edges[4, 0] = countries.denmark;
            edges[4, 1] = countries.germany;

            edges[5, 0] = countries.france;
            edges[5, 1] = countries.germany;

            edges[6, 0] = countries.france;
            edges[6, 1] = countries.luximburg;
            
            edges[7, 0] = countries.germany;
            edges[7, 1] = countries.netherlands;

            edges[8, 0] = countries.germany;
            edges[8, 1] = countries.luximburg;

            //constraints

            countryColours[countries.belgum) != countryColours[countries.france);


            //make a choice 
            //belgum black

            countryColours[0] = colours.Black;
            //test


           
        }
    }
}
