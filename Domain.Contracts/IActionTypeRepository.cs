using Domain.Models.Entities;
using LMS.Shared.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface IActionTypeRepository : IRepositoryBase<Activity>
    {
        Task<IPagedList<ActionType>> GetAllAsync(QueryParameters parameters, bool trackChanges = false);

    }
}
