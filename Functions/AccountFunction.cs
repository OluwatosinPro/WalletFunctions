using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WalletFunctions.Models;
using WalletFunctions.Repositories;

namespace WalletFunctions.Functions
{
    public class AccountFunction
    {
        private readonly IAccountRepository accountRepository;

        public AccountFunction(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [FunctionName("getbyid")]
        public async Task<IActionResult> GetById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Recieved a get request");

            int id = int.Parse(req.Query["id"]);

            OperationResponse<Account> result = new OperationResponse<Account>();
            try
            {
                log.LogInformation("Getting from database");

                //Get Transaction
                result = await accountRepository.GetByIdAsync(id);
                if (!result.IsSuccess)
                {
                    var errorMessage = result.Message;

                    log.LogInformation(errorMessage);
                    return new NotFoundObjectResult
                    (
                        new OperationResponse<Account>
                        {
                            IsSuccess = false,
                            Message = errorMessage
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult
            (
                new OperationResponse<Account>
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Value = result.Value
                }
            );
        }
    }
}