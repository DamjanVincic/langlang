using System.Collections.Generic;
using System.IO;
using System.Linq;
using LangLang.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LangLang.Repositories;

public class UserFileRepository : IUserRepository
{
    private const string UserFileName = "users.json";
    private const string UserDirectoryName = "data";

    private int _idCounter = 1;
    private Dictionary<int, User> _users = new();
    
    public UserFileRepository()
    {
        LoadData();
    }
    
    public List<User> GetAll()
    {
        throw new System.NotImplementedException();
    }

    public User GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public void Add(User user)
    {
        throw new System.NotImplementedException();
    }

    public void Update(User user)
    {
        throw new System.NotImplementedException();
    }

    public void Delete(User user)
    {
        throw new System.NotImplementedException();
    }
    
    private void LoadData()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), UserDirectoryName, UserFileName);
        
        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        _users = JsonConvert.DeserializeObject<Dictionary<int, User>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        }) ?? new Dictionary<int, User>();
            
        _idCounter = _users.Keys.Max() + 1;
    }
}