﻿using Exam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public interface IAPIController
    {
        Task<bool> RefreshAsync();

        string GetLoginFromJWT();
    }
}
