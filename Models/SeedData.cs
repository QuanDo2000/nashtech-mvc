using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MyWebApp.Data;

using System;
using System.Linq;

namespace MyWebApp.Models;

public static class SeedData
{
  public static void Initialize(IServiceProvider serviceProvider)
  {
    using (var context = new PersonContext(
      serviceProvider.GetRequiredService<DbContextOptions<PersonContext>>()
    ))
    {
      if (context.Person.Any())
      {
        return;
      }
      context.Person.AddRange(
        new Person
        {
          FirstName = "Anh",
          LastName = "Nguyen Mai",
          Gender = "Female",
          DateOfBirth = new DateTime(2001, 03, 29),
          PhoneNumber = "0123456789",
          BirthPlace = "Ha Tay",
          IsGraduated = false
        },
        new Person
        {
          FirstName = "Huy",
          LastName = "Nguyen Quang",
          Gender = "Male",
          DateOfBirth = new DateTime(2001, 08, 26),
          PhoneNumber = "0123456789",
          BirthPlace = "Bac Giang",
          IsGraduated = false
        },
        new Person
        {
          FirstName = "Thai",
          LastName = "Pham Van",
          Gender = "Male",
          DateOfBirth = new DateTime(2001, 02, 10),
          PhoneNumber = "0123456789",
          BirthPlace = "Ha Noi",
          IsGraduated = true
        },
        new Person
        {
          FirstName = "Hung",
          LastName = "Duong Dinh",
          Gender = "Male",
          DateOfBirth = new DateTime(2001, 08, 09),
          PhoneNumber = "0123456789",
          BirthPlace = "Bac Ninh",
          IsGraduated = true
        },
        new Person
        {
          FirstName = "Linh",
          LastName = "Do Thuy",
          Gender = "Female",
          DateOfBirth = new DateTime(2001, 04, 21),
          PhoneNumber = "0123456789",
          BirthPlace = "Ha Noi",
          IsGraduated = true
        },
        new Person
        {
          FirstName = "Anh",
          LastName = "Nguyen Thi Phuong",
          Gender = "Female",
          DateOfBirth = new DateTime(2001, 10, 01),
          PhoneNumber = "0123456789",
          BirthPlace = "Ha Noi",
          IsGraduated = true
        }
      );
      context.SaveChanges();
    }
  }
}