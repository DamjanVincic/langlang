﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LangLang.Model;
using Newtonsoft.Json;

namespace LangLang.Repositories;

public class ScheduleFileRepository : IScheduleRepository
{
    private const string ScheduleFileName = "schedule.json";
    private const string ScheduleDirectoryName = "data";
    
    private Dictionary<DateOnly, List<ScheduleItem>> _table = new();

    public List<ScheduleItem> GetByDate(DateOnly date)
    {
        LoadData();
        _table.TryGetValue(date, out var items);
        return items ?? new List<ScheduleItem>();
    }

    public void Add(ScheduleItem item)
    {
        LoadData();
        
        if (!_table.ContainsKey(item.Date))
            _table.Add(item.Date, new List<ScheduleItem>());
        
        _table[item.Date].Add(item);
        
        SaveData();
    }
    
    public void Update(ScheduleItem item)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Deletes the item from the whole schedule
    /// </summary>
    /// <param name="item">The item to delete</param>
    public void Delete(ScheduleItem item)
    {
        LoadData();
        foreach (DateOnly date in _table.Keys)
        {
            List<ScheduleItem> scheduleItems = _table[date];
            scheduleItems.RemoveAll(scheduleItem => scheduleItem.Id == item.Id);
            if (!scheduleItems.Any())
                _table.Remove(date);
        }
        SaveData();
    }
    
    private void SaveData()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), ScheduleDirectoryName, ScheduleFileName);
        
        string json = JsonConvert.SerializeObject(_table, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        });
        
        File.WriteAllText(filePath, json);
    }

    private void LoadData()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), ScheduleDirectoryName, ScheduleFileName);
        
        if (!File.Exists(filePath))
            return;
        
        string json = File.ReadAllText(filePath);

        _table = JsonConvert.DeserializeObject<Dictionary<DateOnly, List<ScheduleItem>>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        }) ?? new Dictionary<DateOnly, List<ScheduleItem>>();
    }
}