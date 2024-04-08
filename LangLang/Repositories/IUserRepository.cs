﻿using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Repositories;

public interface IUserRepository
{
    public List<User> GetAll();
    
    public User GetById(int id);
    
    public void Add(User user);
    
    public void Update(User user);
    
    public void Delete(User user);
}