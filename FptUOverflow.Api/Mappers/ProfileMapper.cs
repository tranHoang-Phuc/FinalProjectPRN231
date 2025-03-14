using AutoMapper;
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using FptUOverflow.Infra.EfCore.Models;
using Microsoft.AspNetCore.Identity;

namespace FptUOverflow.Api.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<ApplicationUser, ApplicationUserResponse>();
            CreateMap<Answer, AnswerResponse>();
            CreateMap<QuestionVote, QuestionVoteResponse>();
            CreateMap<Question, QuestionResponse>();
            CreateMap<QuestionTag, QuestionTagResponse>()
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Tag.TagName));
            CreateMap<CreateQuestionRequest, Question>();
            CreateMap<AnswerVote, AnswerVoteResponse>();
            CreateMap<Tag, TagItemResponse>()
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.TagName))
                .ForMember(dest => dest.NumberOfQuestions, opt => opt.MapFrom(src => src.QuestionTags.Count))
                .ForMember(dest => dest.NumberOfQuestionsToday, opt => opt
                    .MapFrom(src => src.QuestionTags.Where(qt => qt.Question.CreatedAt == System.DateTime.Now.Date).Count()))
                .ForMember(dest => dest.NumberOfQuestionThisWeek, opt => opt
                    .MapFrom(src => src.QuestionTags.Where(qt => qt.Question.CreatedAt >= System.DateTime.Now.Date.AddDays(-7)).Count()));
        }
    }
}
