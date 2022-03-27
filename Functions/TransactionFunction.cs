using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WalletFunctions.Repositories;
using WalletFunctions.Models;

namespace WalletFunctions.Functions
{
    public class TransactionFunction
    {
        private readonly IWalletRepository walletRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;

        public TransactionFunction(
            IWalletRepository walletRepository,
           IAccountRepository accountRepository,
           ITransactionRepository transactionRepository
        )
        {
            this.walletRepository = walletRepository;
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
        }

        [FunctionName("createtransaction")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Recieved a create request");

            // read the contents of the posted data into a string
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // use Json.NET to deserialize the posted JSON into a C# dynamic object
            dynamic model = JsonConvert.DeserializeObject(requestBody);

            //Check model validity
            if (model == null)
            {
                return new BadRequestObjectResult
                (
                    new OperationResponse<Transaction>
                    {
                        IsSuccess = false,
                        Message = "The model object is null"
                    }
                );
            }

            OperationResponse<Transaction> result = new OperationResponse<Transaction>();
            try
            {
                log.LogInformation("Saving to database");

                //Create Wallet
                Wallet walletModel = new Wallet()
                {
                    Balance = 1000, //1000 because balance must be greater than or equals the transaction amount
                };
                var result_Wallet = await walletRepository.CreateAsync(walletModel);
                if (!result_Wallet.IsSuccess)
                {
                    var errorMessage = result_Wallet.Message;

                    log.LogError(errorMessage);
                    return new BadRequestObjectResult
                    (
                        new OperationResponse<Transaction>
                        {
                            IsSuccess = false,
                            Message = errorMessage
                        }
                    );
                }

                //Create Account
                Account accounttModel = new Account()
                {
                    WalletId = result_Wallet.Value.Id,
                };
                var result_Account = await accountRepository.CreateAsync(accounttModel);
                if (!result_Account.IsSuccess)
                {
                    var errorMessage = result_Account.Message;

                    log.LogError(errorMessage);
                    return new BadRequestObjectResult
                    (
                        new OperationResponse<Transaction>
                        {
                            IsSuccess = false,
                            Message = errorMessage
                        }
                    );
                }

                //Create Transaction
                Transaction transactionModel = new Transaction()
                {
                    AccountId = result_Account.Value.Id,
                    Amount = model.Amount,
                    Direction = model.Direction
                };
                result = await transactionRepository.CreateAsync(transactionModel);
                if (!result.IsSuccess)
                {
                    var errorMessage = result.Message;

                    log.LogError(errorMessage);
                    return new BadRequestObjectResult
                    (
                        new OperationResponse<Transaction>
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
                new OperationResponse<Transaction>
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Value = result.Value
                }
            );
        }

        [FunctionName("gettransactionbyaccountid")]
        public async Task<IActionResult> GetByAccountId(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Recieved a get request");

            int accountId = int.Parse(req.Query["accountid"]);

            OperationResponse<Transaction> result = new OperationResponse<Transaction>();
            try
            {
                log.LogInformation("Getting from database");

                //Get Transaction
                result = await transactionRepository.GetByAccountIdAsync(accountId);
                if (!result.IsSuccess)
                {
                    var errorMessage = result.Message;

                    log.LogInformation(errorMessage);
                    return new NotFoundObjectResult
                    (
                        new OperationResponse<Transaction>
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
                new OperationResponse<Transaction>
                {
                    IsSuccess = true,
                    Message = result.Message,
                    Value = result.Value
                }
            );
        }
    }
}
