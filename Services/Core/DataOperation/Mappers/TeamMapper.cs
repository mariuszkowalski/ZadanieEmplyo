using DataAccess.Models;
using Services.Core.DataOperation.Models;


namespace Services.Core.DataOperation.Mappers
{
    public static class TeamMapper
    {
        public static TeamDto ToDto(this Team model)
        {
            return new TeamDto
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static Team ToModel(this TeamDto dto)
        {
            return new Team
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
