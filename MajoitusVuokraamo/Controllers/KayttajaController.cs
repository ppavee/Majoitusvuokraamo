using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MajoitusVuokraamo.Entities;
using MajoitusVuokraamo.Services;
using Dapper;

namespace MajoitusVuokraamo.Controllers
{
    public static class KayttajaController
    {
        private static KayttajaService kayttajaService = new KayttajaService();
        private static string Encrypt(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(input));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }

        public static Kayttaja handleLogIn(string username, string password)
        {
            Kayttaja kayttaja = kayttajaService.ReadSingle(username).Result;
            if(kayttaja != null)
            {
                string passwordHashed = Encrypt(password);
                if (passwordHashed == kayttaja.getSalasana())
                    return kayttaja;
            }
            return null;
        }

        public static bool register(string firstname, string lastname, string plainPassword, string email, string phonenumber)
        {
            string hashPassword = Encrypt(plainPassword);

            Kayttaja k = new Kayttaja(firstname, lastname, hashPassword, phonenumber, email);
            string sql = "INSERT INTO Kayttaja (Etunimi, Sukunimi, Salasana, Puhelinnumero, Sahkoposti) VALUES (@Etunimi, @Sukunimi, @Salasana, @Puhelinnumero, @Sahkoposti);";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@Etunimi", k.getEtunimi() },{ "@Sukunimi", k.getSukunimi() },
                { "@Salasana", k.getSalasana() },{ "@Puhelinnumero", k.getPuhelinnumero() },
                {  "@Sahkoposti", k.getSahkoposti() }
            };
            var parameters = new DynamicParameters(dictionary);
            return kayttajaService.Create(sql, parameters).Result;

        }

        public static bool paivitaYhteystiedot(int kayttajaId, string etunimi, string sukunimi, string puhelinnumero, string sahkoposti)
        {
            string sql = "UPDATE Kayttaja SET Etunimi=@Etunimi, Sukunimi=@Sukunimi, Puhelinnumero=@Puhelinnumero, Sahkoposti=@Sahkoposti WHERE Id=@Id;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@Etunimi", etunimi },
                { "@Sukunimi",sukunimi },
                { "@Puhelinnumero", puhelinnumero },
                {  "@Sahkoposti", sahkoposti },
                { "@Id", kayttajaId }
            };
            var parameters = new DynamicParameters(dictionary);
            return kayttajaService.Update(sql, parameters).Result;
        }

        public static string vaihdaSalasana(Kayttaja nykyinenKayttaja, string nykyinenSalasana, string uusiSalasana)
        {
            if(Encrypt(nykyinenSalasana) != nykyinenKayttaja.getSalasana())
            {
                return null;
            }

            string salasana = Encrypt(uusiSalasana);
            string sql = "UPDATE Kayttaja SET Salasana=@Salasana WHERE Id=@Id;";
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "@Salasana", salasana },
                { "@Id", nykyinenKayttaja.getId() }
            };
            var parameters = new DynamicParameters(dictionary);
            if (kayttajaService.Update(sql, parameters).Result)
                return salasana;
            else
                return null;
        }

        public static int laskeKayttajat()
        {
            string sql = "SELECT Count(*) FROM Kayttaja;";
            return kayttajaService.Count(sql).Result;
        }
    }
}
