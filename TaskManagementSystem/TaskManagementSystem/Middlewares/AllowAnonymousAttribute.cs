﻿namespace TaskManagementSystem.Middlewares
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}

