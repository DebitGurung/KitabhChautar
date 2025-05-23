﻿using KitabhChautari.Dto;
using kitabhChauta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitabhChautari.Services
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAllStaff();
        Task<Staff> GetStaffById(int id);
        Task<Staff> CreateStaff(StaffDto dto);
        Task UpdateStaff(int id, StaffDto dto);
        Task DeleteStaff(int id);
        Task<bool> StaffExists(int id);
    }
}