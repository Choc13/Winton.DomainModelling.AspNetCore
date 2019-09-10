using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Winton.DomainModelling;
using Winton.DomainModelling.AspNetCore;

namespace TestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<ValueDto>> Get()
        {
            return new ActionResult<IEnumerable<ValueDto>>(Enumerable.Empty<ValueDto>());
        }

        [HttpGet("{id}")]
        public ActionResult<ValueDto> Get(int id)
        {
            return new ValueDto(13, "Test");
        }

        [HttpPost]
        public ActionResult<Value> Post([FromBody] ValueDto valueDto)
        {
            return Value.Create(valueDto.Data, valueDto.Name).ToActionResult();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ValueDto valueDto)
        {
        }

        public sealed class Value
        {
            public Value(int data, string name, bool foo)
            {
                Data = data;
                Name = name;
                Foo = foo;
            }

            public int Data { get; }

            public bool Foo { get; }

            public string Name { get; }

            internal static Result<Value> Create(int data, string name)
            {
                Validation<int> ValidateData()
                {
                    return new Validator<int>(data)
                        .Expect(d => d % 2 != 0, nameof(Data), "Data must be odd.")
                        .Expect(d => d > 0, nameof(Data), "Data must be positive.")
                        .Validate();
                }

                Validation<string> ValidateName()
                {
                    return new Invalid<string>(new ValidationError(nameof(Name), "Name error."));
                }

                Validation<bool> Foo()
                {
                    return new Invalid<bool>(
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { nameof(Foo), new List<string> { "Test" } }
                        });
                }

                Func<int, string, Value> create = (d, s) => new Value(d, s, true);

                // ValidateData.And(ValidateName).Then(create);

                return create.Apply(ValidateData()).Apply(new Valid<string>(name)).ToResult();
            }
        }

        public class ValueDto
        {
            public ValueDto(int data, string name)
            {
                Data = data;
                Name = name;
            }

            [Required]
            public int Data { get; }

            [Required]
            [MaxLength(10)]
            public string Name { get; }
        }
    }
}