using System;
using System.Collections.Generic;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Data;

public partial class ECommerceDbContext : DbContext
{
    public ECommerceDbContext()
    {
    }

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CreditCardsPurchase> CreditCardsPurchases {  get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<CreditCardFlag> CreditCardFlags { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ExchangeCoupon> ExchangeCoupons { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Neighborhood> Neighborhoods { get; set; }

    public virtual DbSet<PricingGroup> PricingGroups { get; set; }

    public virtual DbSet<PromotionalCoupon> PromotionalCoupons { get; set; }

    public virtual DbSet<PublicplaceType> PublicplaceTypes { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }

    public virtual DbSet<ResidenceType> ResidenceTypes { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Telephone> Telephones { get; set; }

    public virtual DbSet<TelephoneType> TelephoneTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddId).HasName("address_pk");

            entity.ToTable("address");

            entity.Property(e => e.AddId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_address'::regclass)")
                .HasComment("Represents an unique identifying value of an address.")
                .HasColumnName("add_id");
            entity.Property(e => e.AddCep)
                .HasPrecision(8)
                .HasComment("Represents an address Postal Address Code.")
                .HasColumnName("add_cep");
            entity.Property(e => e.AddNbhId)
                .HasPrecision(5)
                .HasColumnName("add_nbh_id");
            entity.Property(e => e.AddNumber)
                .HasPrecision(4)
                .HasComment("Represents an address number.")
                .HasColumnName("add_number");
            entity.Property(e => e.AddObservations)
                .HasMaxLength(300)
                .HasComment("Represents an optional text containing observations regarding an address.")
                .HasColumnName("add_observations");
            entity.Property(e => e.AddShipping)
                .HasPrecision(6, 2)
                .HasComment("Represents the shipping price of an address")
                .HasColumnName("add_shipping");
            entity.Property(e => e.AddPptId)
                .HasPrecision(3)
                .HasColumnName("add_ppt_id");
            entity.Property(e => e.AddPublicPlace)
                .HasMaxLength(50)
                .HasComment("Represents a public place nearby the address.")
                .HasColumnName("add_public_place");
            entity.Property(e => e.AddRstId)
                .HasPrecision(3)
                .HasColumnName("add_rst_id");
            entity.Property(e => e.AddShortPhrase)
                .HasMaxLength(150)
                .HasComment("Represents a short phrase to better indentify an address.")
                .HasColumnName("add_short_phrase");

            entity.HasOne(d => d.AddNbh).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AddNbhId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("address_neighborhood_fk");

            entity.HasOne(d => d.AddPpt).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AddPptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("address_publicplace_type_fk");

            entity.HasOne(d => d.AddRst).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AddRstId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("address_residence_type_fk");

            entity.HasMany(d => d.BadCtms).WithMany(p => p.BadAdds)
                .UsingEntity<Dictionary<string, object>>(
                    "BillingAddress",
                    r => r.HasOne<Customer>().WithMany()
                        .HasForeignKey("BadCtmId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("billing_address_customer_fk"),
                    l => l.HasOne<Address>().WithMany()
                        .HasForeignKey("BadAddId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("billing_address_address_fk"),
                    j =>
                    {
                        j.HasKey("BadAddId", "BadCtmId").HasName("billing_address_pk");
                        j.ToTable("billing_address");
                        j.IndexerProperty<decimal>("BadAddId")
                            .HasPrecision(5)
                            .HasColumnName("bad_add_id");
                        j.IndexerProperty<decimal>("BadCtmId")
                            .HasPrecision(5)
                            .HasColumnName("bad_ctm_id");
                    });

            entity.HasMany(d => d.DadCtms).WithMany(p => p.DadAdds)
                .UsingEntity<Dictionary<string, object>>(
                    "DeliveryAddress",
                    r => r.HasOne<Customer>().WithMany()
                        .HasForeignKey("DadCtmId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("delivery_address_customer_fk"),
                    l => l.HasOne<Address>().WithMany()
                        .HasForeignKey("DadAddId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("delivery_address_address_fk"),
                    j =>
                    {
                        j.HasKey("DadAddId", "DadCtmId").HasName("delivery_address_pk");
                        j.ToTable("delivery_address");
                        j.IndexerProperty<decimal>("DadAddId")
                            .HasPrecision(5)
                            .HasColumnName("dad_add_id");
                        j.IndexerProperty<decimal>("DadCtmId")
                            .HasPrecision(5)
                            .HasColumnName("dad_ctm_id");
                    });
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.BatId).HasName("author_pk");

            entity.ToTable("author");

            entity.Property(e => e.BatId)
                .HasPrecision(4)
                .HasDefaultValueSql("nextval('seq_author'::regclass)")
                .HasComment("Represents an authors unique indentifying value.")
                .HasColumnName("bat_id");
            entity.Property(e => e.BatName)
                .HasMaxLength(75)
                .HasComment("Represents an authors full name.")
                .HasColumnName("bat_name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BokId).HasName("book_pk");

            entity.ToTable("book");

            entity.HasIndex(e => e.BokStcId, "book__idx").IsUnique();

            entity.Property(e => e.BokId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_book'::regclass)")
                .HasComment("Represents an unique identifying value of a book.")
                .HasColumnName("bok_id");
            entity.Property(e => e.BokBarcode)
                .HasPrecision(14)
                .HasComment("Represents a book barcodes numbers.")
                .HasColumnName("bok_barcode");
            entity.Property(e => e.BokBatId)
                .HasPrecision(4)
                .HasColumnName("bok_bat_id");
            entity.Property(e => e.BokDepth)
                .HasPrecision(4, 2)
                .HasComment("Represents a books depth in centimeters.")
                .HasColumnName("bok_depth");
            entity.Property(e => e.BokEdition)
                .HasPrecision(2)
                .HasComment("Represents the books edition.")
                .HasColumnName("bok_edition");
            entity.Property(e => e.BokHeight)
                .HasPrecision(4, 2)
                .HasComment("Represents a books height in centimeters.")
                .HasColumnName("bok_height");
            entity.Property(e => e.BokImgAddress)
                .HasMaxLength(200)
                .HasComment("Represents the local address of a books cover image.")
                .HasColumnName("bok_img_address");
            entity.Property(e => e.BokIsbn)
                .HasPrecision(13)
                .HasComment("Represents a books International Standard Book Number.")
                .HasColumnName("bok_isbn");
            entity.Property(e => e.BokLength)
                .HasPrecision(4, 2)
                .HasComment("Represents a books length in centimeters.")
                .HasColumnName("bok_length");
            entity.Property(e => e.BokPagesAmount)
                .HasPrecision(4)
                .HasComment("Represents the amount of pages a book has.")
                .HasColumnName("bok_pages_amount");
            entity.Property(e => e.BokPblId)
                .HasPrecision(4)
                .HasColumnName("bok_pbl_id");
            entity.Property(e => e.BokPrgId)
                .HasPrecision(3)
                .HasColumnName("bok_prg_id");
            entity.Property(e => e.BokSinopsis)
                .HasMaxLength(1000)
                .HasComment("Represents a short description of what book is about.")
                .HasColumnName("bok_sinopsis");
            entity.Property(e => e.BokStcId)
                .HasPrecision(5)
                .HasColumnName("bok_stc_id");
            entity.Property(e => e.BokTitle)
                .HasMaxLength(100)
                .HasComment("Represents the title of a book.")
                .HasColumnName("bok_title");
            entity.Property(e => e.BokWeight)
                .HasPrecision(6, 2)
                .HasComment("Represents a books weight in grams.")
                .HasColumnName("bok_weight");
            entity.Property(e => e.BokYear)
                .HasPrecision(4)
                .HasComment("Represents the year when the book was published.")
                .HasColumnName("bok_year");

            entity.HasOne(d => d.BokBat).WithMany(p => p.Books)
                .HasForeignKey(d => d.BokBatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_author_fk");

            entity.HasOne(d => d.BokPbl).WithMany(p => p.Books)
                .HasForeignKey(d => d.BokPblId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_publisher_fk");

            entity.HasOne(d => d.BokPrg).WithMany(p => p.Books)
                .HasForeignKey(d => d.BokPrgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_pricing_group_fk");

            entity.HasOne(d => d.BokStc)
                .WithOne(p => p.Book)
                .HasForeignKey<Book>(d => d.BokStcId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_stock_fk");


            entity.HasMany(d => d.BcrBcts).WithMany(p => p.BcrBoks)
                .UsingEntity<Dictionary<string, object>>(
                    "BookCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("BcrBctId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("book_categories_category_fk"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BcrBokId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("book_categories_book_fk"),
                    j =>
                    {
                        j.HasKey("BcrBokId", "BcrBctId").HasName("book_categories_pk");
                        j.ToTable("book_categories");
                        j.IndexerProperty<decimal>("BcrBokId")
                            .HasPrecision(5)
                            .HasColumnName("bcr_bok_id");
                        j.IndexerProperty<decimal>("BcrBctId")
                            .HasPrecision(3)
                            .HasColumnName("bcr_bct_id");
                    });
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CrtId).HasName("cart_pk");

            entity.ToTable("cart");

            entity.HasIndex(e => e.CrtCtmId, "cart__idx").IsUnique();

            entity.Property(e => e.CrtId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_cart'::regclass)")
                .HasComment("Represents a customer carts unique identifying value")
                .HasColumnName("crt_id");
            entity.Property(e => e.CrtCtmId)
                .HasPrecision(5)
                .HasColumnName("crt_ctm_id");

            entity.HasOne(d => d.CrtCtm).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.CrtCtmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_customer_fk");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.CriCrtId, e.CriStcId }).HasName("cart_items_pk");

            entity.ToTable("cart_items");

            entity.Property(e => e.CriCrtId)
                .HasPrecision(5)
                .HasColumnName("cri_crt_id");
            entity.Property(e => e.CriStcId)
                .HasPrecision(5)
                .HasColumnName("cri_stc_id");
            entity.Property(e => e.CriLastTimeAltered)
                .HasComment("Represents the last time a item in a cart was altered.")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("cri_last_time_altered");
            entity.Property(e => e.CriQuantity)
                .HasPrecision(3)
                .HasComment("Represents the amount of instances of a product is are in a cart")
                .HasColumnName("cri_quantity");
            entity.Property(e => e.CriTotalprice)
                .HasPrecision(6, 2)
                .HasComment("Represents the total price of a product in a cart.")
                .HasColumnName("cri_totalprice");

            entity.HasOne(d => d.CriCrt).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CriCrtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_items_cart_fk");

            entity.HasOne(d => d.CriStc).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CriStcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_items_stock_fk");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.BctId).HasName("category_pk");

            entity.ToTable("category");

            entity.Property(e => e.BctId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_category'::regclass)")
                .HasComment("Represents an unique indentifying value of a books category.")
                .HasColumnName("bct_id");
            entity.Property(e => e.BctName)
                .HasMaxLength(30)
                .HasComment("Represents the name of a books category.")
                .HasColumnName("bct_name");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CtyId).HasName("city_pk");

            entity.ToTable("city");

            entity.Property(e => e.CtyId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_city'::regclass)")
                .HasComment("Represents an unique identifying value of a city.")
                .HasColumnName("cty_id");
            entity.Property(e => e.CtyName)
                .HasMaxLength(50)
                .HasComment("Represents a name of a city.")
                .HasColumnName("cty_name");
            entity.Property(e => e.CtySttId)
                .HasPrecision(3)
                .HasColumnName("cty_stt_id");

            entity.HasOne(d => d.CtyStt).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CtySttId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_state_fk");
        });

        modelBuilder.Entity<CreditCardsPurchase>(entity =>
        {
            entity.HasKey(e => new { e.CcpPrcId, e.CcpCrdId });

            entity.ToTable("credit_cards_purchase");

            entity.Property(e => e.CcpPrcId)
                .HasPrecision(5)
                .HasColumnName("ccp_prc_id");

            entity.Property(e => e.CcpCrdId)
                .HasPrecision(5)
                .HasColumnName("ccp_crd_id");

            entity.Property(e => e.CcpAmount)
                .HasPrecision(6,2)
                .HasColumnName("ccp_amount");

            entity.HasOne(d => d.CcpPrc).WithMany(p => p.CreditCards)
                .HasForeignKey(d => d.CcpPrcId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CcpCrd).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.CcpCrdId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CtrId).HasName("country_pk");

            entity.ToTable("country");

            entity.Property(e => e.CtrId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_country'::regclass)")
                .HasComment("Represents a countrys unique identifying value.")
                .HasColumnName("ctr_id");
            entity.Property(e => e.CtrName)
                .HasMaxLength(50)
                .HasComment("Represents a countrys name.")
                .HasColumnName("ctr_name");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CpnId).HasName("coupon_pk");

            entity.ToTable("coupon");

            entity.Property(e => e.CpnId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_coupon'::regclass)")
                .HasComment("Represents an unique indentifying value of a coupon")
                .HasColumnName("cpn_id");
            entity.Property(e => e.CpnDateGen)
                .HasComment("Represents when a coupon was generated.")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("cpn_date_gen");
            entity.Property(e => e.CpnValue)
                .HasPrecision(5, 2)
                .HasComment("Represents how much a coupon is worth.")
                .HasColumnName("cpn_value");
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.CrdId).HasName("credit_card_pk");

            entity.ToTable("credit_card");

            entity.Property(e => e.CrdId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_credit_card'::regclass)")
                .HasComment("Represents a credit cards unique identifying value.")
                .HasColumnName("crd_id");
            entity.Property(e => e.CrdCcfId)
                .HasPrecision(2)
                .HasColumnName("crd_ccf_id");
            entity.Property(e => e.CrdName)
                .HasMaxLength(50)
                .HasComment("Represents a credit cards printed name.")
                .HasColumnName("crd_name");
            entity.Property(e => e.CrdNumber)
                .HasPrecision(12)
                .HasComment("Represents a credit cards number.")
                .HasColumnName("crd_number");
            entity.Property(e => e.CrdSafetyCode)
                .HasPrecision(3)
                .HasComment("Represents a credit cards safety code.")
                .HasColumnName("crd_safety_code");

            entity.HasOne(d => d.CrdCcf).WithMany(p => p.CreditCards)
                .HasForeignKey(d => d.CrdCcfId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("credit_card_credit_card_flag_fk");
        });

        modelBuilder.Entity<CreditCardFlag>(entity =>
        {
            entity.HasKey(e => e.CcfId).HasName("credit_card_flag_pk");

            entity.ToTable("credit_card_flag");

            entity.Property(e => e.CcfId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_credit_card_flag'::regclass)")
                .HasComment("Represents a credit card flags unique identifying value.")
                .HasColumnName("ccf_id");
            entity.Property(e => e.CcfName)
                .HasMaxLength(50)
                .HasComment("Represents a credit card flags name.")
                .HasColumnName("ccf_name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CtmId).HasName("customer_pk");

            entity.ToTable("customer");

            entity.HasIndex(e => e.CtmCrtId, "customer__idx").IsUnique();

            entity.Property(e => e.CtmId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_customer'::regclass)")
                .HasComment("Represents a customers unique indentifying value.")
                .HasColumnName("ctm_id");
            entity.Property(e => e.CtmActive)
                .HasComment("Boolean value which represents if a customers account is active or not.")
                .HasColumnName("ctm_active");
            entity.Property(e => e.CtmAddId)
                .HasPrecision(5)
                .HasComment("Represents a customer's home address.")
                .HasColumnName("ctm_add_id");
            entity.Property(e => e.CtmBirthdate)
                .HasComment("Represents a customers birthdate.")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("ctm_birthdate");
            entity.Property(e => e.CtmCpf)
                .HasPrecision(11)
                .HasComment("Represents a customers CPF.")
                .HasColumnName("ctm_cpf");
            entity.Property(e => e.CtmCrtId)
                .HasPrecision(5)
                .HasColumnName("ctm_crt_id");
            entity.Property(e => e.CtmEmail)
                .HasMaxLength(50)
                .HasComment("Represents a customers e-mail.")
                .HasColumnName("ctm_email");
            entity.Property(e => e.CtmGndId)
                .HasPrecision(2)
                .HasComment("Represents a customer's gender.")
                .HasColumnName("ctm_gnd_id");
            entity.Property(e => e.CtmName)
                .HasMaxLength(50)
                .HasComment("Represents a customers name.")
                .HasColumnName("ctm_name");
            entity.Property(e => e.CtmPass)
                .HasMaxLength(50)
                .HasComment("Represents a customer accounts password.")
                .HasColumnName("ctm_pass");
            entity.Property(e => e.CtmPrefferedCrdId)
                .HasPrecision(5)
                .HasComment("Represents a customers preffered card ID.")
                .HasColumnName("ctm_preffered_crd_id");
            entity.Property(e => e.CtmRanking)
                .HasPrecision(1)
                .HasComment("Represents a customer's a numeric ranking based of their purchase profile.")
                .HasColumnName("ctm_ranking");
            entity.Property(e => e.CtmTlpId)
                .HasPrecision(5)
                .HasColumnName("ctm_tlp_id");

            entity.HasOne(d => d.CtmAdd).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CtmAddId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_address_fk");

            entity.HasOne(d => d.CtmCrt).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.CtmCrtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_cart_fk");

            entity.HasOne(d => d.CtmGnd).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CtmGndId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_gender_fk");

            entity.HasOne(d => d.CtmPrefferedCrd).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CtmPrefferedCrdId)
                .HasConstraintName("customer_credit_card_fk");

            entity.HasOne(d => d.CtmTlp).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CtmTlpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_telephone_fk");

            entity.HasMany(d => d.CtcCrds).WithMany(p => p.CtcCtms)
                .UsingEntity<Dictionary<string, object>>(
                    "CustomerCreditcard",
                    r => r.HasOne<CreditCard>().WithMany()
                        .HasForeignKey("CtcCrdId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("customer_creditcard_credit_card_fk"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CtcCtmId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("customer_creditcard_customer_fk"),
                    j =>
                    {
                        j.HasKey("CtcCtmId", "CtcCrdId").HasName("customer_creditcard_pk");
                        j.ToTable("customer_creditcard");
                        j.IndexerProperty<decimal>("CtcCtmId")
                            .HasPrecision(5)
                            .HasColumnName("ctc_ctm_id");
                        j.IndexerProperty<decimal>("CtcCrdId")
                            .HasPrecision(5)
                            .HasColumnName("ctc_crd_id");
                    });

            entity.HasMany(d => d.PcmCpns).WithMany(p => p.PcmCtms)
                .UsingEntity<Dictionary<string, object>>(
                    "PromotionalcouponCustomer",
                    r => r.HasOne<PromotionalCoupon>().WithMany()
                        .HasForeignKey("PcmCpnId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("promotional_couponv1_promotional_coupon_fk"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("PcmCtmId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("promotional_couponv1_customer_fk"),
                    j =>
                    {
                        j.HasKey("PcmCtmId", "PcmCpnId").HasName("promotional_couponv1_pk");
                        j.ToTable("promotionalcoupon_customer");
                        j.IndexerProperty<decimal>("PcmCtmId")
                            .HasPrecision(5)
                            .HasColumnName("pcm_ctm_id");
                        j.IndexerProperty<decimal>("PcmCpnId")
                            .HasPrecision(5)
                            .HasColumnName("pcm_cpn_id");
                    });
        });

        modelBuilder.Entity<ExchangeCoupon>(entity =>
        {
            entity.HasKey(e => e.XcpId).HasName("exchange_coupon_pk");

            entity.ToTable("exchange_coupon");

            entity.Property(e => e.XcpId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_exchange_coupon'::regclass)")
                .HasComment("Represents an unique indentifying value of a coupon")
                .HasColumnName("xcp_id");
            entity.Property(e => e.XcpCtmId)
                .HasPrecision(5)
                .IsRequired(false)
                .HasComment("Represents the ID of a customer which a exchange coupon belongs to.")
                .HasColumnName("xcp_ctm_id");

            entity.HasOne(d => d.XcpCtm).WithMany(p => p.ExchangeCoupons)
                .HasForeignKey(d => d.XcpCtmId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("exchange_coupon_customer_fk");

            entity.HasOne(d => d.Xcp).WithOne(p => p.ExchangeCoupon)
                .HasForeignKey<ExchangeCoupon>(d => d.XcpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("exchange_coupon_coupon_fk");

            entity.HasMany(d => d.PxcPrcs).WithMany(p => p.PxcCpns)
                .UsingEntity<Dictionary<string, object>>(
                    "ExchangecouponsPurchase",
                    r => r.HasOne<Purchase>().WithMany()
                        .HasForeignKey("PxcPrcId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("exchangecoupons_purchase_purchase_fk"),
                    l => l.HasOne<ExchangeCoupon>().WithMany()
                        .HasForeignKey("PxcCpnId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("exchangecoupons_purchase_exchange_coupon_fk"),
                    j =>
                    {
                        j.HasKey("PxcCpnId", "PxcPrcId").HasName("exchangecoupons_purchase_pk");
                        j.ToTable("exchangecoupons_purchase");
                        j.IndexerProperty<decimal>("PxcCpnId")
                            .HasPrecision(5)
                            .HasColumnName("pxc_cpn_id");
                        j.IndexerProperty<decimal>("PxcPrcId")
                            .HasPrecision(6)
                            .HasColumnName("pxc_prc_id");
                    });
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GndId).HasName("gender_pk");

            entity.ToTable("gender");

            entity.Property(e => e.GndId)
                .HasPrecision(2)
                .HasDefaultValueSql("nextval('seq_gender'::regclass)")
                .HasComment("Represents a genders unique identifying value.")
                .HasColumnName("gnd_id");
            entity.Property(e => e.GndName)
                .HasMaxLength(50)
                .HasComment("Represents a genders name.")
                .HasColumnName("gnd_name");
        });

        modelBuilder.Entity<Neighborhood>(entity =>
        {
            entity.HasKey(e => e.NbhId).HasName("neighborhood_pk");

            entity.ToTable("neighborhood");

            entity.Property(e => e.NbhId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_neighborhood'::regclass)")
                .HasComment("Represents a neighborhoods unique identifying value.")
                .HasColumnName("nbh_id");
            entity.Property(e => e.NbhCtyId)
                .HasPrecision(5)
                .HasColumnName("nbh_cty_id");
            entity.Property(e => e.NbhName)
                .HasMaxLength(50)
                .HasComment("Represents a neighborhoods name.")
                .HasColumnName("nbh_name");

            entity.HasOne(d => d.NbhCty).WithMany(p => p.Neighborhoods)
                .HasForeignKey(d => d.NbhCtyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("neighborhood_city_fk");
        });

        modelBuilder.Entity<PricingGroup>(entity =>
        {
            entity.HasKey(e => e.PrgId).HasName("pricing_group_pk");

            entity.ToTable("pricing_group");

            entity.Property(e => e.PrgId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_pricing_group'::regclass)")
                .HasComment("Represents an unique identifying value of a pricing group.")
                .HasColumnName("prg_id");
            entity.Property(e => e.PrgDescription)
                .HasMaxLength(100)
                .HasComment("Represents a short description of a pricing group.")
                .HasColumnName("prg_description");
            entity.Property(e => e.PrgName)
                .HasMaxLength(30)
                .HasComment("Represents the name of a pricing group.")
                .HasColumnName("prg_name");
            entity.Property(e => e.PrgProfitMargin)
                .HasPrecision(5, 2)
                .HasComment("Represents the profit margin of a pricing group by percentage.")
                .HasColumnName("prg_profit_margin");
        });

        modelBuilder.Entity<PromotionalCoupon>(entity =>
        {
            entity.HasKey(e => e.PcpId).HasName("promotional_coupon_pk");

            entity.ToTable("promotional_coupon");

            entity.Property(e => e.PcpId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_promotional_coupon'::regclass)")
                .HasComment("Represents an unique indentifying value of a coupon")
                .HasColumnName("pcp_id");
            entity.Property(e => e.PcpCode)
                .HasMaxLength(10)
                .HasComment("Represents the code of a promotional coupon.")
                .HasColumnName("pcp_code");

            entity.HasOne(d => d.Pcp).WithOne(p => p.PromotionalCoupon)
                .HasForeignKey<PromotionalCoupon>(d => d.PcpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("promotional_coupon_coupon_fk");
        });

        modelBuilder.Entity<PublicplaceType>(entity =>
        {
            entity.HasKey(e => e.PptId).HasName("publicplace_type_pk");

            entity.ToTable("publicplace_type");

            entity.Property(e => e.PptId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_publicplace_type'::regclass)")
                .HasComment("Represents an unique identifying value of a type of public place")
                .HasColumnName("ppt_id");
            entity.Property(e => e.PptName)
                .HasMaxLength(50)
                .HasComment("Represents a name of a type of public place.")
                .HasColumnName("ppt_name");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PblId).HasName("publisher_pk");

            entity.ToTable("publisher");

            entity.Property(e => e.PblId)
                .HasPrecision(4)
                .HasDefaultValueSql("nextval('seq_publisher'::regclass)")
                .HasComment("Represents the unique identifying value of a publisher.")
                .HasColumnName("pbl_id");
            entity.Property(e => e.PblName)
                .HasMaxLength(50)
                .HasComment("Represents the name of a publisher.")
                .HasColumnName("pbl_name");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PrcId).HasName("purchase_pk");

            entity.ToTable("purchase");

            entity.Property(e => e.PrcId)
                .HasPrecision(6)
                .HasDefaultValueSql("nextval('seq_purchase'::regclass)")
                .HasComment("Represents a purchases unique identifying value.")
                .HasColumnName("prc_id");
            entity.Property(e => e.PrcAddId)
                .HasPrecision(5)
                .HasColumnName("prc_add_id");
            entity.Property(e => e.PrcCppId)
                .HasPrecision(5)
                .HasComment("Represents a promotional coupons foreign key.")
                .HasColumnName("prc_cpp_id");
            entity.Property(e => e.PrcCtmId)
                .HasPrecision(5)
                .HasColumnName("prc_ctm_id");
            entity.Property(e => e.PrcDate)
                .HasComment("Represents when a purchase was made.")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("prc_date");

            entity.HasOne(d => d.PrcAdd).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.PrcAddId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_address_fk");

            entity.HasOne(d => d.PrcCpp).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.PrcCppId)
                .HasConstraintName("purchase_promotional_coupon_fk");

            entity.HasOne(d => d.PrcCtm).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.PrcCtmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_customer_fk");
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => new { e.PciPrcId, e.PciStcId, e.PciStatus }).HasName("purchase_items_pkey");

            entity.ToTable("purchase_items");

            entity.Property(e => e.PciPrcId)
                .HasPrecision(5)
                .HasColumnName("pci_prc_id");
            entity.Property(e => e.PciStcId)
                .HasPrecision(5)
                .HasColumnName("pci_stc_id");
            entity.Property(e => e.PciQuantity)
                .HasPrecision(3)
                .HasColumnName("pci_quantity");
            entity.Property(e => e.PciStatus)
                .HasPrecision(1)
                .HasColumnName("pci_status");
            entity.Property(e => e.PciTotalPrice)
                .HasPrecision(5, 2)
                .HasColumnName("pci_total_price");

            entity.HasOne(d => d.PciPrc).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PciPrcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_items_purchase_fk");

            entity.HasOne(d => d.PciStc).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PciStcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("purchase_items_stock_fk");
        });

        modelBuilder.Entity<ResidenceType>(entity =>
        {
            entity.HasKey(e => e.RstId).HasName("residence_type_pk");

            entity.ToTable("residence_type");

            entity.Property(e => e.RstId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_residence_type'::regclass)")
                .HasComment("Represents an unique identifying value of a residence type.")
                .HasColumnName("rst_id");
            entity.Property(e => e.RstName)
                .HasMaxLength(50)
                .HasComment("Represents the name of a residence type.")
                .HasColumnName("rst_name");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.SttId).HasName("state_pk");

            entity.ToTable("states");

            entity.Property(e => e.SttId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_states'::regclass)")
                .HasComment("Represents a country states unique identifying value.")
                .HasColumnName("stt_id");
            entity.Property(e => e.SttCtrId)
                .HasPrecision(3)
                .HasColumnName("stt_ctr_id");
            entity.Property(e => e.SttName)
                .HasMaxLength(75)
                .HasComment("Represents the name of a countrys state.")
                .HasColumnName("stt_name");

            entity.HasOne(d => d.SttCtr).WithMany(p => p.States)
                .HasForeignKey(d => d.SttCtrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("state_country_fk");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StcId).HasName("stock_pk");

            entity.ToTable("stock");

            entity.HasIndex(e => e.StcBokId, "stock__idx").IsUnique();

            entity.Property(e => e.StcId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_stock'::regclass)")
                .HasComment("Represents an unique identifying value of a product in stock.")
                .HasColumnName("stc_id");
            entity.Property(e => e.StcAvailableAmount)
                .HasPrecision(4)
                .HasComment("Represents the amount of items of a product in stock available for purchase.")
                .HasColumnName("stc_available_amount");
            entity.Property(e => e.StcBlockedAmount)
                .HasPrecision(4)
                .HasComment("Represents the amount of items of a product in stock which are in a customer's cart.")
                .HasColumnName("stc_blocked_amount");
            entity.Property(e => e.StcBokId)
                .HasPrecision(5)
                .HasComment("Represents which books stock is been referenced in a row.")
                .HasColumnName("stc_bok_id");
            entity.Property(e => e.StcCost)
                .HasPrecision(6, 2)
                .HasComment("Represents a products cost value in dollars.")
                .HasColumnName("stc_cost");
            entity.Property(e => e.StcEntryDate)
                .HasComment("Represents when a product was registered.")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("stc_entry_date");
            entity.Property(e => e.StcRemoveInterval)
                .HasPrecision(3)
                .HasComment("Represents, in minutes, how long it takes until an item is removed from a cart.")
                .HasColumnName("stc_remove_interval");
            entity.Property(e => e.StcSppId)
                .HasPrecision(3)
                .HasComment("Represents a products supplier unique identifying value.")
                .HasColumnName("stc_spp_id");

            entity.HasOne(d => d.StcBok).WithOne(p => p.Stock)
                .HasForeignKey<Stock>(d => d.StcBokId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_book_fk");

            entity.HasOne(d => d.StcSpp).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StcSppId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stock_supplier_fk");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SppId).HasName("supplier_pk");

            entity.ToTable("supplier");

            entity.Property(e => e.SppId)
                .HasPrecision(3)
                .HasDefaultValueSql("nextval('seq_supplier'::regclass)")
                .HasComment("Represents a suppliers unique identifying value.")
                .HasColumnName("spp_id");
            entity.Property(e => e.SppName)
                .HasMaxLength(50)
                .HasComment("Represents a suppliers name.")
                .HasColumnName("spp_name");
        });

        modelBuilder.Entity<Telephone>(entity =>
        {
            entity.HasKey(e => e.TlpId).HasName("telephone_pk");

            entity.ToTable("telephone");

            entity.Property(e => e.TlpId)
                .HasPrecision(5)
                .HasDefaultValueSql("nextval('seq_telephone'::regclass)")
                .HasComment("Represents a telephones unique identifying value.")
                .HasColumnName("tlp_id");
            entity.Property(e => e.TlpDdd)
                .HasPrecision(3)
                .HasComment("Represents a telephones DDD.")
                .HasColumnName("tlp_ddd");
            entity.Property(e => e.TlpNumber)
                .HasMaxLength(9)
                .HasComment("Represents a telephones number.")
                .HasColumnName("tlp_number");
            entity.Property(e => e.TlpTptId)
                .HasPrecision(2)
                .HasColumnName("tlp_tpt_id");

            entity.HasOne(d => d.TlpTpt).WithMany(p => p.Telephones)
                .HasForeignKey(d => d.TlpTptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("telephone_telephone_type_fk");
        });

        modelBuilder.Entity<TelephoneType>(entity =>
        {
            entity.HasKey(e => e.TptId).HasName("telephone_type_pk");

            entity.ToTable("telephone_type");

            entity.Property(e => e.TptId)
                .HasPrecision(2)
                .HasDefaultValueSql("nextval('seq_telephone_type'::regclass)")
                .HasComment("Represents a telephone types unique identifying value.")
                .HasColumnName("tpt_id");
            entity.Property(e => e.TptName)
                .HasMaxLength(50)
                .HasComment("Represents a telephone types name.")
                .HasColumnName("tpt_name");
        });
        modelBuilder.HasSequence("seq_address");
        modelBuilder.HasSequence("seq_author");
        modelBuilder.HasSequence("seq_book");
        modelBuilder.HasSequence("seq_cart");
        modelBuilder.HasSequence("seq_category");
        modelBuilder.HasSequence("seq_city");
        modelBuilder.HasSequence("seq_country");
        modelBuilder.HasSequence("seq_coupon");
        modelBuilder.HasSequence("seq_credit_card");
        modelBuilder.HasSequence("seq_credit_card_flag");
        modelBuilder.HasSequence("seq_customer");
        modelBuilder.HasSequence("seq_exchange_coupon");
        modelBuilder.HasSequence("seq_gender");
        modelBuilder.HasSequence("seq_neighborhood");
        modelBuilder.HasSequence("seq_pricing_group");
        modelBuilder.HasSequence("seq_promotional_coupon");
        modelBuilder.HasSequence("seq_publicplace_type");
        modelBuilder.HasSequence("seq_publisher");
        modelBuilder.HasSequence("seq_purchase");
        modelBuilder.HasSequence("seq_residence_type");
        modelBuilder.HasSequence("seq_states");
        modelBuilder.HasSequence("seq_stock");
        modelBuilder.HasSequence("seq_supplier");
        modelBuilder.HasSequence("seq_telephone");
        modelBuilder.HasSequence("seq_telephone_type");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
