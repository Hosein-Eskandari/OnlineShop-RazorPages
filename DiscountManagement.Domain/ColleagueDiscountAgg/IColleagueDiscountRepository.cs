﻿using System.Collections.Generic;
using _0_Framework.Domain;
using DiscountManagement.Application.Contracts.ColleagueDiscount;

namespace DiscountManagement.Domain.ColleagueDiscountAgg;

public interface IColleagueDiscountRepository : IRepository<long, ColleagueDiscount>
{
    EditColleagueDiscount GetDetails(long id);

    List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel search);
}