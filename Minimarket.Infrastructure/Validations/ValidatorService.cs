﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infraestructure.Validations
{
    public interface IValidatorService
    {
        Task<ValidationResult> ValidateAsync<T>(T instance);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
    public class ValidationService : IValidatorService
    {
        private readonly IServiceProvider _serviceProvider;
        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<ValidationResult> ValidateAsync<T>(T model)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
            {
                throw new InvalidOperationException($"Validación No encontrada para el tipo {typeof(T).Name}");
            }

            var result = await validator.ValidateAsync(model);

            return new ValidationResult
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }
    }
}
