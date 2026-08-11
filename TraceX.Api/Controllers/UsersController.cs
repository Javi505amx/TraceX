using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TraceX.Application.DTOs.Users;
using TraceX.Domain.Entities;
using TraceX.Domain.Interfaces;

namespace TraceX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUserDto> _createUserDtoValidator;
        private readonly IValidator<UpdateUserDto> _updateUserDtoValidator;

        public UsersController(IUserRepository userRepository, IValidator<CreateUserDto> createUserDtoValidator
            , IValidator<UpdateUserDto> updateUserDtoValidator)
        {
            _userRepository = userRepository;
            _createUserDtoValidator = createUserDtoValidator;
            _updateUserDtoValidator = updateUserDtoValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Adapt<List<UserDto>>();
            return Ok(userDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            var userDto = user.Adapt<UserDto>();
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            var validationResult = await _createUserDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            var existingUser = await _userRepository.GetByEmployeeNumberAsync(dto.EmployeeNumber);
            if (existingUser != null)
                return Conflict(new { Message = "There's already an employee registered with this Empolyee Number" });

            var userEntity = dto.Adapt<User>();
            var createdUser = await _userRepository.AddAsync(userEntity);
            var responseDto = createdUser.Adapt<UserDto>();

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = responseDto.Id },
                responseDto);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var validationResult = await _updateUserDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser is null) return NotFound();

            if (existingUser.Email != dto.Email && await _userRepository.ExistsByEmailAsync(dto.Email))
            {
                return Conflict(new { Message = $"The email '{dto.Email}' is already registered by another user." });
            }

            dto.Adapt(existingUser);

            await _userRepository.UpdateAsync(existingUser);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMachine(int id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            await _userRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
