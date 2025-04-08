using System.Data;

namespace Models;
class SQLParameter{
    public string key;
    public object? value;
    public SqlDbType type;
    public SQLParameter(string key, SqlDbType type, object? value){
        if (value != null){
            this.key = key;
            this.type = type;
            this.value = value;
            
        }else{
            throw new Exception("Not Valid Value for Parameter");
        }
        
    }
}