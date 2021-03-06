﻿using API.Hostings.ServiceInitialization.Abstraction;
using API.Services.Implementation;
using System;

namespace API.Services.Abstraction
{
    public interface IValidatingService: IOnInitService
    {
        void Validate(string[] attrs, object obj, string objName);
        void Validate<T>(T dto);
        void Validate(Type type, object dto);
        bool IsIssetValidateFunc(string funcId);
        void AddValidateFunc<T>(string funcId, ValidateFunc<T> func);
    }
}
