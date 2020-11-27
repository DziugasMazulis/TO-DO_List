using AutoMapper;
using TO_DO_List.Models;
using TO_DO_List.Models.Dto;
using TO_DO_List.ViewModels;

namespace TO_DO_List.Data
{
    public class ToDoTaskProfile : Profile
    {
        public ToDoTaskProfile()
        {
            CreateMap<ToDoTask, ToDoTaskResponse>()
                .ForMember(dest =>
                dest.Username,
                opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<ToDoTaskRequest, ToDoTask>()
                .ForMember(dest =>
                dest.User,
                opt => opt.Ignore());

            CreateMap<ToDoTask, ToDoTask>()
                .ForMember(dest =>
                dest.User,
                opt => opt.Ignore());
        }
    }
}
