using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bibliotheek.Domain.Entities
{
    public class IsIsbnAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string isbn = value as string;
            if (string.IsNullOrEmpty(isbn)) {
                return false;
            }
            bool result = false;
            if (isbn.Length == 13)
            {
                if (isbn.Substring(0, 3) == "978" || isbn.Substring(0, 3) == "979")
                {
                    //controle isbn13
                    int controleGetal = Convert.ToInt32(isbn.Substring(12));
                    string basis = isbn.Remove(12);
                    int controle = 99;
                    int someven = 0, somoneven = 0, som = 0, afgerSom = 0;
                    for (int i = 1; i < basis.Length + 1; i++)
                    {
                        if ((i % 2) == 0)
                        {
                            someven += Convert.ToInt32(basis[i - 1].ToString());
                        }
                        else
                        {
                            somoneven += Convert.ToInt32(basis[i - 1].ToString());
                        }
                    }
                    som = (someven * 3) + somoneven;
                    if ((som % 10) == 0)
                    {
                        controle = 0;
                    }
                    else
                    {
                        afgerSom = (((int)(som / 10)) * 10) + 10;
                        controle = afgerSom - som;
                    }

                    if (controleGetal == controle)
                        result = true;
                }

            }
            else if (isbn.Length == 10)
            {
                //controle isbn10
                string strBasis = isbn.Substring(0, 9);
                string strControle = isbn.Substring(9);

                Int32 intControle;
                if (strControle == "X")
                    intControle = 10;
                else
                    if (!Int32.TryParse(strControle, out intControle))
                        return result;

                int som = 0;
                for (int i = 0; i < strBasis.Length; i++)
                    som += Int32.Parse(strBasis[i].ToString()) * (i + 1);
                int modulo = som % 11;

                if (intControle == modulo)
                    result = true;
            }
            return result;
        }
    }
}
