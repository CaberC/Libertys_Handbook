using Microsoft.Data.SqlClient;
namespace Program{
    class Person{
        //string types is char[255] types in database
        private string UserName;
        private string Email;
        private string Password;
        private int Zip;
        private float Range;
        public Person(string UserName, string Email, string Password, int Zip, float Range){
            this.UserName = UserName;
            this.Email = Email;
            this.Password = Password;
            this.Zip = Zip;
            this.Range = Range;
        }
        public void SetUserName(string UserName){
            this.UserName = UserName;
        }
        public void SetEmail(string Email){
            this.Email = Email;
        }
        public void SetPassword(string Password){
            this.Password = Password;
        }
        public void SetZip(int Zip){
            this.Zip = Zip;
        }
        public void SetRange(float Range){
            this.Range = Range;
        }
        public string GetUserName(){
            return UserName;
        }
        public string GetEmail(){
            return Email;
        }
        public string GetPassword(){
            return Password;
        }
        public int GetZip(){
            return Zip;
        }
        public float GetRange(){
            return Range;
        }

        public bool Create(){
            try{
                Start sqlConn = new Start();
                string maxStr = sqlConn.UseConn("SELECT MAX(ID) FROM person;");
                string[] strArr = SplitReader(maxStr);
                int maxInt;
                if (int.TryParse(strArr[0], out maxInt)){
                    sqlConn.UseConn("INSERT INTO person VALUES(ID = "+(maxInt++)+" UserName = "+UserName+" Email = "+Email+" Password = "+Password+" Zip = "+Zip+" Range = "+Range+" );");
                }else{
                    throw new Exception("NOT AN INTEGER "+maxStr);
                }
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public int GetID(){
            try{
                Start sqlConn = new Start();
                string str = sqlConn.UseConn("SELECT * FROM person WHERE UserName = "+UserName+" AND Email = "+Email+";");
                string[] strArr = SplitReader(str);
                str = strArr[0];
                int maxInt;
                if (int.TryParse(str, out maxInt)){
                    return maxInt;
                }else{
                    throw new Exception("NOT AN INTEGER "+str);
                }   
            }catch(Exception e){
                Console.WriteLine(e);
                return -1;
            }
        }
        public bool Read(int ID){
            try{
                Start sqlConn = new Start();
                string str = sqlConn.UseConn("SELECT * FROM person WHERE ID = "+ID+";");
                string[] strArr = SplitReader(str);
                UserName = strArr[1];
                Email = strArr[2];
                Password = strArr[3];
                Zip = int.Parse(strArr[4]);
                Range = float.Parse(strArr[5]);
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        public bool Update(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("UPDATE person SET UserName = "+UserName+" Email = "+Email+" Password = "+Password+" Zip = "+Zip+" Range = "+Range+" WHERE ID = "+ID+";");
                return true;   
            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        //ADD DELETE FROM ALL TABLES AS THEY'RE ADDED
        public bool Delete(int ID){
            try{
                Start sqlConn = new Start();
                sqlConn.UseConn("DELETE FROM person WHERE ID = "+ID+" AND UserName = "+UserName+";");
                return true;   

            }catch(Exception e){
                Console.WriteLine(e);
                return false;
            }
        }
        private string[] SplitReader(string str){
            string[] sub = str.Split(',');
            string[] outStr = new string[sub.Length*2];

            int i = 0;
            foreach(string s in sub){
                int index = s.IndexOf(" \n");
                if (index>0) s.Remove(index);
                index = s.IndexOf(':')+2;
                string subStr = s.Substring(index);
                outStr[i] = subStr;
                i++;
            }

            return outStr;
        }
    }
}