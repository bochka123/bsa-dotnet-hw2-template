using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CoolParking.Presentation.ViewModels
{
    public class VehicleViewModel
    {
        [NotNull]
        [RegularExpression(@"[A-Z]{2}-\d{4}-[A-Z]{2}")]
        public string Id { get; }
        public VehicleTypeViewModel VehicleType { get; }
        [Range(0, double.MaxValue)]
        public decimal Balance { get; internal set; }

        public VehicleViewModel(string Id, VehicleTypeViewModel VehicleType, decimal Balance)
        {
            this.Id = Id;
            this.VehicleType = VehicleType;
            this.Balance = Balance;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            if (!Validator.TryValidateObject(this, context, results, true))
                throw new ArgumentException();
        }
        public static string GenerateRandomRegistrationPlateNumber()
        {
            var random = new Random();
            char s1 = (char)random.Next('A', 'Z' + 1);
            char s2 = (char)random.Next('A', 'Z' + 1);
            int s3 = random.Next(0, 10);
            int s4 = random.Next(0, 10);
            int s5 = random.Next(0, 10);
            int s6 = random.Next(0, 10);
            char s7 = (char)random.Next('A', 'Z' + 1);
            char s8 = (char)random.Next('A', 'Z' + 1);
            return $"{s1}{s2}-{s3}{s4}{s5}{s6}-{s7}{s8}";
        }
        public override string ToString()
        {
            return $"Vehicle #{Id}: {VehicleType} with {Balance} balance";
        }
    }
}
