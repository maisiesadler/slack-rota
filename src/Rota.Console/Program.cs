using Microsoft.Extensions.DependencyInjection;
using Rota.Console;
using Rota.Domain;

var serviceProvider = Services.BuildServiceProvider();

var updateRotaInteractor = serviceProvider.GetRequiredService<UpdateRotaInteractor>();

await updateRotaInteractor.Execute();

