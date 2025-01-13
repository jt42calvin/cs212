//////////////////////////////
///
/// Jacob Tocila CS 212 Dutch Bingo Program
/// Start date: Nov 9, 2023
/// Totaale uuren gewerkt: 11/9 4:00 PM tot 6:30 PM; 11/10 12:00 PM tot 2:15 PM; 11/10 4:00 PM tot 5:00 PM
/// I got the following functions complete:
///         orphans, siblings, descendants, and bingo
/// I did not do the extra credit, besides defining the function hahaha
/// 
//////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;

namespace Bingo
{
    class Program
    {
        private static RelationshipGraph rg;

        // Read RelationshipGraph whose filename is passed in as a parameter.
        // Build a RelationshipGraph in RelationshipGraph rg
        private static void ReadRelationshipGraph(string filename)
        {
            rg = new RelationshipGraph();                           // create a new RelationshipGraph object

            string name = "";                                       // name of person currently being read
            int numPeople = 0;
            string[] values;
            Console.Write("Reading file " + filename + "\n");
            try
            {
                string input = System.IO.File.ReadAllText(filename);// read file
                input = input.Replace("\r", ";");                   // get rid of nasty carriage returns 
                input = input.Replace("\n", ";");                   // get rid of nasty new lines
                string[] inputItems = Regex.Split(input, @";\s*");  // parse out the relationships (separated by ;)
                foreach (string item in inputItems)
                {
                    if (item.Length > 2)                            // don't bother with empty relationships
                    {
                        values = Regex.Split(item, @"\s*:\s*");     // parse out relationship:name
                        if (values[0] == "name")                    // name:[personname] indicates start of new person
                        {
                            name = values[1];                       // remember name for future relationships
                            rg.AddNode(name);                       // create the node
                            numPeople++;
                        }
                        else
                        {
                            rg.AddEdge(name, values[1], values[0]); // add relationship (name1, name2, relationship)

                            // handle symmetric relationships -- add the other way
                            if (values[0] == "hasSpouse" || values[0] == "hasFriend")
                                rg.AddEdge(values[1], name, values[0]);

                            // for parent relationships add child as well
                            else if (values[0] == "hasParent")
                                rg.AddEdge(values[1], name, "hasChild");
                            else if (values[0] == "hasChild")
                                rg.AddEdge(values[1], name, "hasParent");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Unable to read file {0}: {1}\n", filename, e.ToString());
            }
            Console.WriteLine(numPeople + " people read");
        }

        // Show the relationships a person is involved in
        private static void ShowPerson(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
                Console.Write(n.ToString());
            else
                Console.WriteLine("{0} not found", name);
        }

        // Show a person's friends
        private static void ShowFriends(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
            {
                Console.Write("{0}'s friends: ", name);
                List<GraphEdge> friendEdges = n.GetEdges("hasFriend");
                foreach (GraphEdge e in friendEdges) {
                    Console.Write("\t \n{0} ", e.To());
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine("{0} not found", name);
        }

        // Show all people without parents
        private static void ShowOrphans()
        {
            Console.Write("Orphans: \n");
            // Loops through every person, checking the amount of parents each person has. If they have none, print their name.
            foreach (GraphNode n in rg.nodes)
            {
                if (n.GetEdges("hasParent").Count == 0)
                    Console.Write("\t" + n.Name + "\n");
                // "\t" is does a tab in Console.Write
            }
        }

        // Show all persons siblings (look at their parents and print all their children)
        private static void ShowSiblings(string name)
        {
            GraphNode node = rg.GetNode(name);
            if (node != null)
            {
                Console.Write("\t{0}'s siblings: ", name);
                // This gets the nodes related to the parent
                List<GraphEdge> parentEdges = node.GetEdges("hasParent");

                // For every node that the parent is related to...
                List<GraphNode> parentNodes = new List<GraphNode>();
                foreach (GraphEdge p in parentEdges)
                {
                    parentNodes.Add(p.ToNode());
                }

                // Looking at all of the nodes, if they are siblings, print them out in console
                foreach(GraphNode p in parentNodes)
                {
                    // Get children (sounds weird...)
                    List<GraphEdge> childEdges = p.GetEdges("hasChild");
                    foreach (GraphEdge e in childEdges)
                    {
                        if (e.To() != name)
                        {
                            Console.Write("{0}" + " ", e.To());
                            
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("{0} does not exist", name);
            }
        }

        // Show a person's decendents
        private static void ShowDecendents(string name)
        {
            // Get the root node
            GraphNode root = rg.GetNode(name);
            if ((root == null))
            {
                {
                    Console.WriteLine("{0} does not exist", name);
                    return;
                }
            }
            // List to store descendant nodes
            List<GraphNode> descendantNodes = new List<GraphNode>();
            descendantNodes.Add(root);
            // List to store the next level of descendant nodes
            List<GraphNode> nextDescendantNodes = new List<GraphNode>();
            int generation = 0;
            try
            {
                // Check if the root has children first
                if (root.GetEdges("hadChild").Count == 0)
                {
                    Console.WriteLine("\t{0} has no children", name);
                }
                else
                {
                    Console.Write("\t{0}'s descendants: \n", name);
                    while (descendantNodes.Count > 0)
                    {
                        List<string> children = new List<string>();

                        foreach (GraphNode node in descendantNodes)
                        {
                            foreach (GraphEdge edge in node.GetEdges("hasChild"))
                            {
                                nextDescendantNodes.Add(edge.ToNode());
                                children.Add(edge.To());
                            }
                        }
                        // Print descendants based on generation
                        if (generation == 0 && children.Count > 0)
                        {
                            Console.Write("Children: ");
                            Console.WriteLine(string.Join(", ", children));
                        }
                        else if (generation > 0 && children.Count > 0)
                        {
                            Console.Write($"Great {new string('G', generation - 1)}children: ");
                            Console.WriteLine(string.Join(", ", children));
                        }
                        // Update descendant nodes for the next generation
                        descendantNodes = nextDescendantNodes;
                        nextDescendantNodes = new List<GraphNode>();
                        generation++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to read:");
                Console.WriteLine(e.Message);
            }
        }

        // Show all of a persons Nth-cousins K times removed
        private static void ShowCousins(string name, int n, int k)
        {
            // this function is optional
        }

        // Show the shortest path from two people
        // If I'm being quite honest, I needed a lot of help with this function. I'm still trying to understand it all. I will explain it the best I can with comments
        // The function as a whole using a BFS to find the shortest path from one person to another
        private static List<GraphNode> GimmeBingo(string FromPerson, string ToPerson)
        {
            // We are finding the shortest path from FromPerson to ToPerson
            GraphNode From = rg.GetNode(FromPerson);
            GraphNode To = rg.GetNode(ToPerson);
            // First check if the people exist, or if we are checking the same person twice
            if (From == null || To == null || From == To)
            {
                return null;
            }
            else
            {
                // Create a queue to handle paths and levels in BFS
                List<GraphNode> path = new List<GraphNode>();
                List<List<GraphNode>> levels = new List<List<GraphNode>>();
                List<GraphNode> level = new List<GraphNode>();
                Dictionary<string, bool> check = new Dictionary<string, bool>();

                // initialize path and levels to work with on our BFS
                level.Add(From);
                levels.Add(level);
                int levelCount = 0;

                // The actual BFS
                while (true)
                {
                    level = new List<GraphNode>();
                    // Here we explore each node at the current level of search
                    foreach (GraphNode node in levels[levels.Count - 1])
                    {
                        // Every neighboring node from the current node
                        foreach (GraphEdge edge in node.GetEdges())
                        {
                            if (edge.To() == ToPerson)
                            {
                                // Here we found the ToPerson
                                goto EndBuild;
                            }
                            if (!check.ContainsKey(edge.To()))
                                // In this loop we look at if the neighbor isn't visited
                                // Add the neighbor into the next level and mark them as visited using the bool variable
                            {
                                level.Add(edge.ToNode());
                                check.Add(edge.To(), true);
                            }
                        }
                    }
                    // Outside the loop...
                    levels.Add(level);
                }
            // I really like this function of C#! I think it's really handy for niche case situations like this,
            // but I'm worried it could lead to some functions having issues or processes being skipped if misused
            EndBuild:
                path.Add(To);
                levelCount = levels.Count - 1;
                //Rebuild the path from ToPerson to FromPerson (backwards, yes)
                while (path[path.Count - 1] != From && levelCount >= 0)
                {
                    foreach (GraphEdge edge in path[path.Count - 1].GetEdges())
                    {
                        if (levels[levelCount].Contains(edge.ToNode()))
                        {
                            path.Add(edge.ToNode());
                            // Keep going in the loop
                            goto NextInWhileLoop;
                        }
                    }
                    // Check nodes for reverse edges
                    foreach (GraphNode node in levels[levelCount])
                    {
                        foreach (GraphEdge edge in node.GetEdges())
                        {
                            // If a reverse edge is found, add the node into the path
                            if (edge.ToNode() == path[path.Count - 1])
                            {
                                path.Add(node);
                                goto NextInWhileLoop;
                                // Keep going in the loop
                            }
                        }
                    }
                NextInWhileLoop:
                    levelCount--;
                }
                // Very important! We reverse the path so that we get the path in the right order in printing
                path.Reverse();
                if (path.Count == 1)
                    // If the path has only 1 person, themself (shouldn't happen because the if statement in the beginning of the program)
                {
                    path = null;
                }
                return path;
            }
        }


        // accept, parse, and execute user commands
        private static void CommandLoop()
        {
            string command = "";
            string[] commandWords;
            Console.Write("Welcome to Harry's Dutch Bingo Parlor!\n");

            while (command != "exit")
            {
                Console.Write("\nEnter a command: ");
                command = Console.ReadLine();
                commandWords = Regex.Split(command, @"\s+");        // split input into array of words
                command = commandWords[0];

                if (command == "exit")
                    ;                                               // do nothing

                // read a relationship graph from a file
                else if (command == "read" && commandWords.Length > 1)
                    ReadRelationshipGraph(commandWords[1]);

                // show information for one person
                else if (command == "show" && commandWords.Length > 1)
                    ShowPerson(commandWords[1]);

                else if (command == "friends" && commandWords.Length > 1)
                    ShowFriends(commandWords[1]);

                // dump command prints out the graph
                else if (command == "dump")
                    rg.Dump();

                // prints all orphans
                else if (command == "orphans")
                    ShowOrphans();

                // show all siblings of a person
                else if (command == "siblings" && commandWords.Length > 1)
                    ShowSiblings(commandWords[1]);

                // shows all descendants of a person
                else if (command == "descendants" && commandWords.Length > 1)
                    ShowDecendents(commandWords[1]);
                //
                // TODO: fix this function, and add other remaining functions from master file
                // remaining functions: siblings, descendants
                //

                else if (command == "bingo" && commandWords.Length > 1)
                {
                    try
                    {
                        if (rg.GetNode(commandWords[1]) != null && rg.GetNode(commandWords[2]) != null)
                        {
                            List<GraphNode> path = GimmeBingo(commandWords[1], commandWords[2]);

                            for (int i = 1; i < path.Count; i++)
                            {
                                foreach (GraphEdge graphEdge in path[i - 1].GetEdges())
                                {
                                    if (graphEdge.ToNode() == path[i])
                                    {
                                        // I initially had these two people swapped. So it was saying that parents were children of their children lol
                                        Console.WriteLine(path[i - 0].GetName() + " is a " + graphEdge.GetLabel().Substring(3) + " of " + path[i - 1].GetName());
                                    }
                                }
                            }
                        }
                        else
                            Console.WriteLine(commandWords[1] + " is unrelated to " + commandWords[2]);
                    }
                    catch (Exception e)
                    {
                        Console.Write("Unknown person\n");
                        Console.WriteLine(e.Message);
                    }
                }


                // cleans screen
                else if (command == "clear")
                {
                    Console.Clear();
                    Console.Write("Welcome to Harry's Dutch Bingo Parlor!\n");
                    Console.Write("\nEnter a command: ");
                    Console.Write("\nLegal commands:\nread [filename]\ndump\nshow [person_name]\nfriends [person_name]\norphans\ndecendants [person_name]\ncousins [person_name N K]\nbingo [from_person_name] [to_person_name]\nclear\nexit\n");
                }

                // illegal command
                else
                    Console.Write("\nLegal commands:\nread [filename]\ndump\nshow [person_name]\nfriends [person_name]\norphans\ndecendants [person_name]\ncousins [person_name N K]\nbingo [from_person_name] [to_person_name]\nclear\nexit\n");
            }
        }

        static void Main(string[] args)
        {
            CommandLoop();
        }
    }
}
//bruh