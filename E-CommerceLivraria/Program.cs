using E_CommerceLivraria.Data;
using E_CommerceLivraria.Extra;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Repository.AddressR.RegionsR;
using E_CommerceLivraria.Repository.CreditCardR;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Repository.CustomerR.CouponR;
using E_CommerceLivraria.Repository.CustomerR.GenderR;
using E_CommerceLivraria.Repository.CustomerR.TelephoneR;
using E_CommerceLivraria.Repository.PurchaseR;
using E_CommerceLivraria.Repository.StockR;
using E_CommerceLivraria.Repository.StockR.BookR;
using E_CommerceLivraria.Repository.StockR.BookR.AuthorR;
using E_CommerceLivraria.Repository.StockR.BookR.CategoryR;
using E_CommerceLivraria.Repository.StockR.BookR.PricingGroupR;
using E_CommerceLivraria.Repository.StockR.BookR.PublisherR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.AddressS.RegionsS;
using E_CommerceLivraria.Services.CouponS;
using E_CommerceLivraria.Services.CreditCardS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.CustomerS.TelephoneS;
using E_CommerceLivraria.Services.PurchaseS;
using E_CommerceLivraria.Services.StockS;
using E_CommerceLivraria.Services.StockS.BookS;
using E_CommerceLivraria.Services.StockS.BookS.AuthorS;
using E_CommerceLivraria.Services.StockS.BookS.CategoryS;
using E_CommerceLivraria.Services.StockS.BookS.PricingGroupS;
using E_CommerceLivraria.Services.StockS.BookS.PublisherS;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ECommerceDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped
);

builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(
        new NamespaceRoutingConvention()
    ));
});

// Repositórios e Serviços
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IGenderRepository, GenderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IExchangeCouponRepository, ExchangeCouponRepository>();
builder.Services.AddScoped<IExchangeCouponService, ExchangeCouponService>();

builder.Services.AddScoped<ITelephoneRepository, TelephoneRepository>();
builder.Services.AddScoped<ITelephoneService, TelephoneService>();
builder.Services.AddScoped<ITelephoneTypeRepository, TelephoneTypeRepository>();
builder.Services.AddScoped<ITelephoneTypeService, TelephoneTypeService>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IPublicPlaceTypeRepository, PublicPlaceTypeRepository>();
builder.Services.AddScoped<IResidenceTypeRepository, ResidenceTypeRepository>();

builder.Services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
builder.Services.AddScoped<INeighborhoodService, NeighborhoodService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPricingGroupRepository, PricingGroupRepository>();
builder.Services.AddScoped<IPricingGroupService, PricingGroupService>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockService, StockService>();

builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPurchaseItemRepository, PurchaseItemRepository>();
builder.Services.AddScoped<IPurchaseItemService, PurchaseItemService>();

builder.Services.AddScoped<ICreditCardRepository, CreditCardRepository>();
builder.Services.AddScoped<ICreditCardService, CreditCardService>();
builder.Services.AddScoped<ICreditCardFlagsRepository, CreditCardFlagsRepository>();
builder.Services.AddScoped<ICreditCardFlagService, CreditCardFlagService>();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=searchbar}/");

app.Run();
