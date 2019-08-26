using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomPasswords_PA
{
    class Program
    {
        class Password
        {
            private readonly int mPassSize;
            private readonly int mNumberOfMayus;
            private readonly int mNumberOfSpecChars;
            public Password(int passSize, int numberOfMayus, int numberOfSpecChars)
            {
                mPassSize = passSize;
                mNumberOfMayus = numberOfMayus;
                mNumberOfSpecChars = numberOfSpecChars;
            }
            private List<string> GeneratePassword(int numberOfPasswords)
            {
                Random rnd = new Random();
                List<string> validPasswords = new List<string>();
                char randomLetter;
                string avaliableChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!#$%&'()*+,-./:;=?@";
                for (int i = 0; i < numberOfPasswords; i++)
                {
                    string genPass = "";
                    int[] arraySelect = { 1 };
                    int randomOption;
                    int contChars = mPassSize;
                    int contNormal = 0;
                    int contSpec = 0;
                    int contMayus = 0;
                    if (mNumberOfMayus > 0 && mNumberOfSpecChars > 0)
                    {
                        Array.Resize(ref arraySelect, 3);
                        arraySelect[0] = 1;
                        arraySelect[1] = 2;
                        arraySelect[2] = 3;
                        contMayus = mNumberOfMayus;
                        contSpec = mNumberOfSpecChars;
                    }
                    else if (mNumberOfMayus == 0 && mNumberOfSpecChars > 0)
                    {
                        Array.Resize(ref arraySelect, 2);
                        arraySelect[0] = 1;
                        arraySelect[1] = 3;
                        contSpec = mNumberOfSpecChars;
                    }
                    else if (mNumberOfMayus > 0 && mNumberOfSpecChars == 0)
                    {
                        Array.Resize(ref arraySelect, 2);
                        arraySelect[0] = 1;
                        arraySelect[1] = 2;
                        contMayus = mNumberOfMayus;
                    }
                    contNormal = contChars - contMayus - contSpec;
                    while (contChars > 0)
                    {
                        randomOption = arraySelect[rnd.Next(arraySelect.Length)];
                        if (randomOption == 1 && contNormal > 0)
                        {
                            randomLetter = avaliableChars[rnd.Next(0, 35)];
                            genPass += randomLetter.ToString();
                            contChars--;
                        }
                        else if (randomOption == 2 && contMayus > 0)
                        {
                            randomLetter = avaliableChars[rnd.Next(36, 61)];
                            genPass += randomLetter.ToString();
                            contChars--;
                            contMayus--;
                        }
                        else if (randomOption == 3 && contSpec > 0)
                        {
                            randomLetter = avaliableChars[rnd.Next(62, 80)];
                            genPass += randomLetter.ToString();
                            contChars--;
                            contSpec--;
                        }
                    }
                    Predicate<string> isValid = FindPoints;
                     bool FindPoints(string a)
                    {
                        return a == genPass;
                    }
                    if (!validPasswords.Exists(isValid))
                    {
                        validPasswords.Add(genPass);
                    }
                    else
                    {
                        i--;
                    }
                }
                return validPasswords;
            }

            public List<string> EncryptPasswds(int interval, int numOfPass)
            {
                List<string> encryptedPass = GeneratePassword(numOfPass);
              
                // Recorrer la lista encryptedPass
                for (int i = 0; i < numOfPass; i++)
                {
                    //Recorrer cada contraseña dentro de la lista
                    for (int j = 0; j < encryptedPass[i].Length; j++)
                    {
                        //Los strings son inmutables por lo que necesitamos crear un arreglo de tipo char
                        char[] p = encryptedPass[i].ToArray();
                        //Extraemos el valor ASCII del caracter
                        int asciiValue = Convert.ToInt32(p[j]);
                        //Modificamos el valor para ser cambiado con base al intervalo
                        asciiValue += interval;
                        if(asciiValue > 122)
                        {
                            asciiValue -= 90;
                        }
                        //Lo insertamos en el espacio correspondiente
                        p[j] = Convert.ToChar(asciiValue);
                        //Reemplazamos en la lista el viejo string por la nueva contraseña encriptada
                        encryptedPass[i] = new string(p);  
                    }

                }
                return encryptedPass;

            }
            public List<string> DecryptPasswds(int interval, List<string> encrypted)
            {
                List<string> decryptedPass = encrypted;

                // Recorrer la lista encryptedPass
                for (int i = 0; i < decryptedPass.Count; i++)
                {
                    //Recorrer cada contraseña dentro de la lista
                    for (int j = 0; j < decryptedPass[i].Length; j++)
                    {
                        //Los strings son inmutables por lo que necesitamos crear un arreglo de tipo char
                        char[] p = decryptedPass[i].ToArray();
                        //Extraemos el valor ASCII del caracter
                        int asciiValue = Convert.ToInt32(p[j]);
                        //Modificamos el valor para ser cambiado con base al intervalo
                        asciiValue -= interval;
                        if (asciiValue < 33)
                        {
                            asciiValue += 90;
                        }
                        //Lo insertamos en el espacio correspondiente
                        p[j] = Convert.ToChar(asciiValue);
                        //Reemplazamos en la lista el viejo string por la nueva contraseña encriptada
                        decryptedPass[i] = new string(p);
                    }

                }
                return decryptedPass;
            }
        }

        static void Main(string[] args)
        {


            int numberOfPasswords;
            int sizeOfPass;
            int numOfMayus;
            int numOfSpecs;

            //La salida en consola será la siguiente:
            //1.Mostrar en pantalla Cuantos passwords desea generar?
            Console.WriteLine("Cuantos passwords desea generar?");
            //2.Leer el numero de contaseñas y almecenarlo en numberOfPasswords
            numberOfPasswords = Convert.ToInt32(Console.ReadLine());
            if (numberOfPasswords == 0) return;
            //3.Mostrar en pantalla De que tamaño será cada password?
            Console.WriteLine("De que tamaño será cada password?");
            //4.Leer el tamaño de la contraseña y almacenarlo en mPassSize
            sizeOfPass = Convert.ToInt32(Console.ReadLine());
            //Se comprueba que la longitud de la contraseña sea mayor a cero
            if(sizeOfPass < 1)
            {
                Console.Clear();
                Console.WriteLine("La longitud de la contraseña debe ser mayor a cero");
                //Pedir passSize hasta que se ingrese un valor válido
                do
                {
                    sizeOfPass = Convert.ToInt32(Console.ReadLine());
                } while (sizeOfPass < 1);
            }
            //5.Mostrar en pantalla "Cuántas mayúsculas debo poner en los passwords?"
            Console.WriteLine("Cuántas mayúsculas debo poner en los passwords?");
            //6.Leer el numero de mayusculas y almacenarlo en numOfMayus
            numOfMayus = Convert.ToInt32(Console.ReadLine());
            //Validar que el numero de mayusculas sea menor a la longitud de la contraseña
            if (numOfMayus > sizeOfPass)
            {
                Console.WriteLine("El número de mayusculas no debe sobrepasar la longitud de la contraseña\nIngrese el número de mayúsculas:");
                //Pedir el numero de mayusculas hasta que se ingrese un valor válido
                do
                {
                    numOfMayus = Convert.ToInt32(Console.ReadLine());
                } while (numOfMayus > sizeOfPass);
            }

            //7.Cuántos caracteres especiales debo poner en los passwords?
            Console.WriteLine("Cuántos caracteres especiales debo poner en los passwords?");
            //8.Leer el numero de caracteres especiales y almacenarlo en mNumberOfSpecChars;
            numOfSpecs = Convert.ToInt32(Console.ReadLine());
            if(numOfSpecs > sizeOfPass)
            {
                Console.WriteLine("El número de caracteres especiales no debe sobrepasar la longitud de la contraseña");
                Console.WriteLine("Ingrese el numero de caracteres especiales:");
                //Pedir el numero de especiales hasta que se ingrese un valor válido
                do
                {
                    numOfSpecs = Convert.ToInt32(Console.ReadLine());
                } while (numOfSpecs > sizeOfPass);
            }
            else if (numOfSpecs + numOfMayus > sizeOfPass)
            {
                Console.WriteLine("La suma del numero de Caracteres especiales y el número de Mayusculas no puede exceder a la longitud de la contraseña");
                Console.WriteLine("Ingrese el numero de caracteres especiales:");
                //Pedir el numero de especiales hasta que se ingrese un valor válido
                do
                {
                    numOfSpecs = Convert.ToInt32(Console.ReadLine());
                } while (numOfSpecs > sizeOfPass);
            }

            Password newpassword = new Password(sizeOfPass,numOfMayus,numOfSpecs);
            List<string> finalPasswords = newpassword.EncryptPasswds(3, numberOfPasswords);
            Console.WriteLine("\nContraseñas Encriptadas");
            foreach (string actualEncriptado in finalPasswords)
            {
                Console.WriteLine(actualEncriptado);
            }
            Console.WriteLine("\nContraseñas Desencriptadas");
            foreach (string actualDesencriptado in newpassword.DecryptPasswds(3,finalPasswords))
            {
                Console.WriteLine(actualDesencriptado);
            }
            Console.Read();
            //9.Imprime el N número de passwords
            //10.Imprime el N número de passwords encriptados
            //11.Imprime el N número de passwords desencriptados


        }
    }
}
