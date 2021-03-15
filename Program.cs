using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        private int valueLeft;
        private int valueRight;

        private string operatorSign;
        private List<string> existingOperatorList;

        private int result;

        public void StartConsole()
        {
             existingOperatorList = this.SetexistingOperatorList();

            Console.WriteLine("Bienvenue dans l'outil calculatrice \n");

            Console.WriteLine("Si vous avez besoin d'aide, utilisez la commande /help");
            Console.WriteLine("Si vous souhaitez la calculatrice, utilisez la commande /quit \n");

            do
            {
                List<Object> listConcat = complexCalculator();
                this.SetResult("complex", listConcat);
                //simpleCalculator();
                //this.SetResult("simple");

                Console.WriteLine("Le résultat est : " + this.result + "\n\n");
            } while (true);
            

        }

        private List<Object> complexCalculator()
        {
            List<Object> listValue = new List<Object>();
            List<Object> listOperator = new List<Object>();

            bool verifValue;
            int commandState = 0;

            do
            {
                string strValue;
                int value;

                do { 
                    Console.WriteLine("Veuillez entrer une valeur : ");
                    strValue = Console.ReadLine();
                    Console.WriteLine("Saisie : " + strValue);
                    verifValue = int.TryParse(strValue, out value);
                    commandState = this.UseCommand(strValue);
                    if(strValue == "(")
                    {
                        break;
                    }
                } while (!verifValue || commandState == 1);

                if (strValue == "(")
                {
                    Console.WriteLine("Condition (");

                    existingOperatorList.Add(")");

                    List<Object> listParenthèses = complexCalculator();
                    listValue.Add(listParenthèses[0]);
                    listOperator.Add(listParenthèses[1]);
                }
                else
                {
                    listValue.Add(value);
                }
                Console.WriteLine("Liste value : " + String.Join(";",listValue));


                do
                {
                    Console.WriteLine("Veuillez entrer l'opérateur (+, -, /, *, %, =) : ");
                    this.operatorSign = Console.ReadLine();

                    commandState = this.UseCommand(this.operatorSign);
                } while (!existingOperatorList.Contains(operatorSign) || commandState == 1);

                if(this.operatorSign == ")")
                {
                    Console.WriteLine("( Supprimée");
                    existingOperatorList.Remove(")");
                    break;
                }
                else if(this.operatorSign == "=")
                {
                    break;
                }
                else
                {
                    listOperator.Add(this.operatorSign);
                    Console.WriteLine("Liste operator : " + String.Join(";", listOperator));
                }
                


            } while (this.operatorSign != "=");
                

            Console.WriteLine("Sortie de boucle");

            List<Object> listConcat = new List<Object>();
            listConcat.Add(listValue);
            listConcat.Add(listOperator);
            return listConcat;
        }


        private void simpleCalculator()
        {
            bool verifValueLeft, verifValueRight;
            int commandState = 0;
            do
            {
                Console.WriteLine("Veuillez entrer une première valeur : ");
                string strValueLeft = Console.ReadLine();

                verifValueLeft = int.TryParse(strValueLeft, out this.valueLeft);
                commandState = this.UseCommand(strValueLeft);
            } while (!verifValueLeft || commandState == 1);


            do
            {
                Console.WriteLine("Veuillez entrer l'opérateur (+, -, /, *, %) : ");
                this.operatorSign = Console.ReadLine();

                commandState = this.UseCommand(this.operatorSign);
            } while (!existingOperatorList.Contains(operatorSign) || commandState == 1);


            do
            {
                Console.WriteLine("Veuillez entrer une deuxième valeur : ");
                string strValueRight = Console.ReadLine();

                verifValueRight = int.TryParse(strValueRight, out this.valueRight);
                commandState = this.UseCommand(strValueRight);
            } while (!verifValueRight || commandState == 1);
        }

        // TO-DO
        private int UseCommand(string str)
        {
            if (str.Equals("/quit"))
            {
                /* if (System.Windows.Forms.Application.MessageLoop)
                {
                    // WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                { */
                    // Console app
                    System.Environment.Exit(1);
               // }
               // return 1;
            }
            else if (str.Equals("/help"))
            {
                Console.WriteLine("Si vous souhaitez nettoyer l'affichage, tapez /clear \n");
                Console.WriteLine("Si vous souhaitez quitter l'application, tapez /quit \n");
                return 1;
            }
            else if (str.Equals("/clear"))
            {
                Console.Clear();
                return 1;
            }
            return 0;
        }

        private List<string> SetexistingOperatorList()
        {
            List<string> existingOperatorList = new List<string>();
            existingOperatorList.Add("+");
            existingOperatorList.Add("-");
            existingOperatorList.Add("*");
            existingOperatorList.Add("/");
            existingOperatorList.Add("%");
            existingOperatorList.Add("=");

            return existingOperatorList;
        }

        private int SetResult(string state, List<Object> listConcat = null)
        {
            listConcat = listConcat ?? new List<Object>();

            if (state.Equals("simple"))
            {
                switch (this.operatorSign)
                {
                    case "+":
                        this.result = this.valueLeft + this.valueRight;
                        break;

                    case "-":
                        this.result = this.valueLeft - this.valueRight;
                        break;

                    case "*":
                        this.result = this.valueLeft * this.valueRight;
                        break;

                    case "/":
                        this.result = this.valueLeft / this.valueRight;
                        break;

                    case "%":
                        this.result = this.valueLeft % this.valueRight;
                        break;

                    default:
                        break;
                }
            }
            else if(state.Equals("complex"))
            {
                List<Object> listValue = (List<object>)listConcat[0];
                List<Object> listOperator = (List<object>)listConcat[1];

                // Priorité aux parenthèses
                while (listValue.OfType<List<Object>>().Any())
                {
                    Console.WriteLine("Opérateur ( et )");
                    List<Object> sousListConcat = new List<object>();
                    //List<Object> sousListValue = listValue.OfType<List<object>>().ToList();
                    //List<Object>[] sousListOperator = listOperator.OfType<List<object>>().ToArray();
                
                    sousListConcat.Add(listValue.OfType<List<object>>().ToList()[0]);
                    sousListConcat.Add(listOperator.OfType<List<object>>().ToList()[0]);
                    this.result = SetResult("complex", sousListConcat);

                    int idxVal = listValue.IndexOf(listValue.OfType<List<object>>().ToList()[0]);
                    listValue.RemoveAt(idxVal);
                    listValue.Insert(idxVal, this.result);

                    int idxOp = listOperator.IndexOf(listOperator.OfType<List<object>>().ToList()[0]);
                    listOperator.RemoveAt(idxOp);
                   
                }

                // Priorité à la multiplication et à la division
                while(listOperator.Contains("*") || listOperator.Contains("/"))
                {
                    Console.WriteLine("Opérateur * et /");
                    int idxX = -1, idxDiv = -1;
                    if (listOperator.Contains("*"))
                    {
                        idxX = listOperator.FindIndex(x => (string)x == "*");
                        Console.WriteLine(idxX);

                    }
                    if (listOperator.Contains("/"))
                    {
                        idxDiv = listOperator.FindIndex(x => (string)x == "/");
                        Console.WriteLine(idxDiv);

                    }

                    if ((idxX < idxDiv && idxDiv!=-1 && idxX != -1) || idxX != -1)
                    {
                        listConcat = otherStepOfCalculation(listConcat, idxX);

                    }
                    else if((idxDiv < idxX && idxX !=1 && idxDiv!=1) || idxDiv != -1)
                    {
                        listConcat = otherStepOfCalculation(listConcat, idxDiv);
                    }
                    listValue = (List<object>)listConcat[0];
                    listOperator = (List<object>)listConcat[1];
                }

                for(int i=0; i < listOperator.Count(); i++)
                {
                    Console.WriteLine("Opérateur + et -" + i);
                    listConcat = otherStepOfCalculation(listConcat, i);
                }
                
            }
            return this.result;
        } 

        private List<Object> otherStepOfCalculation(List<Object> list, int i)
        {
            List<Object> listValue = (List<Object>)list[0];
            List<Object> listOperator = (List<Object>)list[1];
            List<Object> listConcat = new List<Object>();

            this.valueLeft = (int)listValue[i];
            this.valueRight = (int)listValue[i + 1];
            this.operatorSign = (string)listOperator[i];

            this.result = SetResult("simple");

            listValue.RemoveAt(i + 1);
            listValue.RemoveAt(i);
            listValue.Insert(i, this.result);

            listOperator.RemoveAt(i);

            listConcat.Add(listValue);
            listConcat.Add(listOperator);

            return listConcat;
        }

        static void Main(string[] args)
        {
            Program calc = new Program();
            calc.StartConsole();
            
        }
    }
}
