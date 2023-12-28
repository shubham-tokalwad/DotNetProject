
using AutoMapper;
using refactor_me.Models;
using refactor_me.Models.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace refactor_me.Services
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductDTO, Product>().ForMember(dest => dest.IsNew, opt => opt.MapFrom(src => true));
                cfg.CreateMap<ProductOptionDTO, ProductOption>();

            });
        }
    }
}