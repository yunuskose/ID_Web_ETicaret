USE [DATABASE]
GO
/****** Object:  UserDefinedFunction [dbo].[StringArray]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[StringArray]
(
	@ParamaterList VARCHAR(MAX),
    @Delimiter CHAR(1)
)
RETURNS @ReturnList TABLE
(
  FieldValue VARCHAR(MAX)
)
AS BEGIN
    DECLARE @ArrayList TABLE
        (
          FieldValue VARCHAR(MAX)
        )
  
        
    DECLARE @Value VARCHAR(MAX)
    DECLARE @CurrentPosition INT
 
    SET @ParamaterList = LTRIM(RTRIM(@ParamaterList))
        + CASE WHEN RIGHT(@ParamaterList, 1) = @Delimiter THEN ''
               ELSE @Delimiter
          END
    SET @CurrentPosition = ISNULL(CHARINDEX(@Delimiter, @ParamaterList, 1), 0)  

    IF @CurrentPosition = 0
        INSERT  INTO @ArrayList ( FieldValue )
                SELECT  @ParamaterList
    ELSE
        BEGIN
            WHILE @CurrentPosition > 0
                BEGIN
                    SET @Value = LTRIM(RTRIM(LEFT(@ParamaterList,
                                                  @CurrentPosition - 1))) --make sure a value exists between the delimiters
                    IF LEN(@ParamaterList) > 0
                        AND @CurrentPosition <= LEN(@ParamaterList)
                        BEGIN
                            INSERT  INTO @ArrayList ( FieldValue )
                                    SELECT  @Value
                        END
                    SET @ParamaterList = SUBSTRING(@ParamaterList,
                                                   @CurrentPosition
                                                   + LEN(@Delimiter),
                                                   LEN(@ParamaterList))
                    SET @CurrentPosition = CHARINDEX(@Delimiter,
                                                     @ParamaterList, 1)
                END
        END
      
    INSERT  @ReturnList ( FieldValue )
       
			SELECT FieldValue
            FROM    @ArrayList
			UNION ALL 
			SELECT ISNULL((SELECT TOP(1) Dogru FROM YanlisKelimeler YK(NOLOCK) WHERE YK.Yanlis = FieldValue),'XXX')
            FROM    @ArrayList
			
			

    RETURN
   END
GO
/****** Object:  UserDefinedFunction [dbo].[Temizle]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE FUNCTION [dbo].[Temizle] (@HTMLText VARCHAR(MAX))
RETURNS VARCHAR(MAX) AS
BEGIN
    DECLARE @Start INT
    DECLARE @End INT
    DECLARE @Length INT
    SET @Start = CHARINDEX('<',@HTMLText)
    SET @End = CHARINDEX('>',@HTMLText,CHARINDEX('<',@HTMLText))
    SET @Length = (@End - @Start) + 1
    WHILE @Start > 0 AND @End > 0 AND @Length > 0
    BEGIN
        SET @HTMLText = STUFF(@HTMLText,@Start,@Length,'')
        SET @Start = CHARINDEX('<',@HTMLText)
        SET @End = CHARINDEX('>',@HTMLText,CHARINDEX('<',@HTMLText))
        SET @Length = (@End - @Start) + 1
    END
    SET @HTMLText = LOWER(@HTMLText)
    SET @HTMLText = REPLACE(@HTMLText,'ğ','g')
    SET @HTMLText = REPLACE(@HTMLText,'ü','u')
    SET @HTMLText = REPLACE(@HTMLText,'ş','s')
    SET @HTMLText = REPLACE(@HTMLText,'ı','i')
    SET @HTMLText = REPLACE(@HTMLText,'ö','o')
    SET @HTMLText = REPLACE(@HTMLText,'ç','c')
	
    SET @HTMLText = REPLACE(@HTMLText,'/','-')
    SET @HTMLText = REPLACE(@HTMLText,'*','-')
    SET @HTMLText = REPLACE(@HTMLText,'+','-')
    SET @HTMLText = REPLACE(@HTMLText,'&','-')
    RETURN LOWER(LTRIM(RTRIM(@HTMLText)))
END
GO
/****** Object:  UserDefinedFunction [dbo].[UpperFirstCharacter]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

	CREATE FUNCTION [dbo].[UpperFirstCharacter]
(
@word nvarchar(500)
)
RETURNS nvarchar(500)
AS
BEGIN
return @word

RETURN stuff((
select ' '+upper(left(T3.V, 1)) + lower(stuff(T3.V, 1, 1, ''))
from (select cast(replace((select @Word as '*' for xml path('')), ' ', '<X/>') as xml).query('.')) as T1(X)
cross apply T1.X.nodes('text()') as T2(X)
cross apply (select T2.X.value('.', 'varchar(30)')) as T3(V)
for xml path(''), type
).value('text()[1]', 'varchar(100)'), 1, 1, '');

END
GO
/****** Object:  Table [dbo].[Account]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](250) NULL,
	[Password] [varchar](250) NULL,
	[FullName] [varchar](250) NULL,
	[Address] [varchar](250) NULL,
	[Email] [varchar](250) NULL,
	[Token] [varchar](250) NULL,
	[Phone] [varchar](50) NULL,
	[Status] [bit] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Table_1] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Adresler]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Adresler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[AdresIsmi] [nvarchar](max) NULL,
	[Isim] [nvarchar](max) NULL,
	[CepTelefonu] [nvarchar](max) NULL,
	[Il] [nvarchar](max) NULL,
	[Ilce] [nvarchar](max) NULL,
	[Adres] [nvarchar](max) NULL,
	[TC] [nvarchar](max) NULL,
	[FaturaTuru] [nvarchar](max) NULL,
	[FirmaAdi] [nvarchar](max) NULL,
	[VergiDairesi] [nvarchar](max) NULL,
	[VergiNumarasi] [nvarchar](max) NULL,
	[KayitTarihi] [datetime] NULL,
 CONSTRAINT [PK_Adresler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNet_SqlCacheTablesForChangeNotification]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNet_SqlCacheTablesForChangeNotification](
	[tableName] [nvarchar](450) NOT NULL,
	[notificationCreated] [datetime] NOT NULL,
	[changeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[tableName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bankalar]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bankalar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Sirket] [nvarchar](max) NULL,
	[Banka] [nvarchar](max) NULL,
	[VergiNumarasi] [nvarchar](max) NULL,
	[TerminalID] [nvarchar](max) NULL,
	[MarchantID] [nvarchar](max) NULL,
	[TerminalUserID] [nvarchar](max) NULL,
	[BankName] [nvarchar](max) NULL,
	[ClientID] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[ProvisionPassword] [nvarchar](max) NULL,
	[PosUrl] [nvarchar](max) NULL,
	[okURL] [nvarchar](max) NULL,
	[failURL] [nvarchar](max) NULL,
	[receiveURL] [nvarchar](max) NULL,
	[Aktif] [bit] NULL,
 CONSTRAINT [PK_Bankalar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankaXml]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankaXml](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[form] [nvarchar](max) NULL,
	[CDate] [datetime] NULL,
 CONSTRAINT [PK_BankaXml] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Card]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Card](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[VendorId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantity] [int] NULL,
	[Aktarilacak] [bit] NULL,
	[CDate] [datetime] NULL,
 CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[ParentId] [int] NULL,
	[VendorId] [int] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CVV]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CVV](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[CDate] [datetime] NULL,
 CONSTRAINT [PK_CVV] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HomeProducts]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HomeProducts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProdutID] [int] NULL,
	[Type] [nvarchar](50) NULL,
 CONSTRAINT [PK_HomeProducts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ilceler]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ilceler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Isim] [nvarchar](100) NULL,
	[Kod] [int] NULL,
	[SehirID] [int] NULL,
 CONSTRAINT [PK_Ilceler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KullanicilarXml]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KullanicilarXml](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Isim] [nvarchar](200) NULL,
	[IP] [nvarchar](100) NULL,
	[KayitTarihi] [datetime] NULL,
 CONSTRAINT [PK_KullanicilarXml] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loglar]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loglar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tip] [nvarchar](max) NULL,
	[Kullanici] [nvarchar](max) NULL,
	[Tarih] [datetime] NULL,
	[Aciklama] [nvarchar](max) NULL,
 CONSTRAINT [PK_Loglar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberShip]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberShip](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Description] [ntext] NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Month] [int] NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_MemberShip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberShipVendor]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberShipVendor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemerShipId] [int] NOT NULL,
	[VendorId] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_MemberShipVendor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](250) NULL,
	[Body] [ntext] NULL,
	[DateCreation] [date] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[VendorId] [int] NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[DateCreation] [date] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[VendorId] [int] NOT NULL,
	[OrderStatusId] [int] NOT NULL,
	[PaymentId] [int] NULL,
	[Aktarildi] [bit] NULL,
	[Email] [varchar](250) NULL,
	[CDate] [datetime] NULL,
	[Isim] [varchar](250) NULL,
	[CepTelefonu] [varchar](250) NULL,
	[Il] [varchar](250) NULL,
	[Ilce] [varchar](250) NULL,
	[Adres] [varchar](250) NULL,
	[TC] [varchar](250) NULL,
	[FaturaTuru] [varchar](250) NULL,
	[FirmaAdi] [varchar](250) NULL,
	[VergiDairesi] [varchar](250) NULL,
	[VergiNumarasi] [varchar](250) NULL,
	[OrderID] [varchar](250) NULL,
	[Ertele] [bit] NULL,
	[Aktif] [bit] NULL,
	[Kargo] [nvarchar](50) NULL,
	[KargoNo] [nvarchar](50) NULL,
	[Aciklama1] [nvarchar](max) NULL,
	[Aciklama2] [nvarchar](max) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdersDetail]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersDetail](
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrdersDetail] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdersDetailLog]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersDetailLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CDate] [datetime] NULL,
	[Kullanici] [nvarchar](max) NULL,
 CONSTRAINT [PK_OrdersDetailLog_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrdersLog]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrdersLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[DateCreation] [date] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[VendorId] [int] NOT NULL,
	[OrderStatusId] [int] NOT NULL,
	[PaymentId] [int] NULL,
	[Aktarildi] [bit] NULL,
	[CDate] [datetime] NULL,
	[Isim] [varchar](250) NULL,
	[CepTelefonu] [varchar](250) NULL,
	[Il] [varchar](250) NULL,
	[Ilce] [varchar](250) NULL,
	[Adres] [varchar](250) NULL,
	[TC] [varchar](250) NULL,
	[FaturaTuru] [varchar](250) NULL,
	[FirmaAdi] [varchar](250) NULL,
	[VergiDairesi] [varchar](250) NULL,
	[VergiNumarasi] [varchar](250) NULL,
	[OrderID] [nvarchar](250) NULL,
	[Email] [varchar](250) NULL,
	[Ertele] [bit] NULL,
 CONSTRAINT [PK_OrdersLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Page]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Page](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Plug] [varchar](250) NULL,
	[Title] [varchar](250) NULL,
	[Detail] [ntext] NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Photo]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Photo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NULL,
	[Status] [bit] NOT NULL,
	[Main] [bit] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PosOdemeleri]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PosOdemeleri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tip] [nvarchar](50) NULL,
	[Tarih] [datetime] NULL,
	[GonderilenTutar] [decimal](18, 2) NULL,
	[txnamount] [decimal](18, 2) NULL,
	[xid] [nvarchar](max) NULL,
	[hostmsg] [nvarchar](max) NULL,
	[taksitAdet] [int] NULL,
	[bankaAd] [nvarchar](max) NULL,
	[authcode] [nvarchar](max) NULL,
	[hostrefnum] [nvarchar](max) NULL,
	[rnd] [nvarchar](max) NULL,
	[procreturncode] [nvarchar](max) NOT NULL,
	[transid] [nvarchar](max) NULL,
	[mode] [nvarchar](max) NULL,
	[response] [nvarchar](max) NULL,
	[successurl] [nvarchar](max) NULL,
	[errmsg] [nvarchar](max) NULL,
	[md] [nvarchar](max) NULL,
	[oid] [nvarchar](max) NULL,
	[hash] [nvarchar](max) NULL,
	[txntimestamp] [nvarchar](max) NULL,
	[customeripaddress] [nvarchar](max) NULL,
	[terminalid] [nvarchar](max) NULL,
	[orderid] [nvarchar](max) NULL,
	[MaskedPan] [nvarchar](max) NULL,
	[Isim] [nvarchar](max) NULL,
	[secure3dhash] [nvarchar](max) NULL,
	[UserID] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[KayitYapanKullanici] [nvarchar](max) NOT NULL,
	[KayitTarihi] [datetime] NULL,
	[DuzenlemeTarihi] [datetime] NULL,
	[KontrolEdildi] [bit] NULL,
	[Aciklama] [nvarchar](max) NULL,
	[Aktarildi] [bit] NULL,
	[SonucDegeri] [nvarchar](max) NULL,
	[GercekSiparisNo] [nvarchar](max) NULL,
 CONSTRAINT [PK_PosOdemeleri] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](250) NULL,
	[Name] [varchar](250) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Description] [ntext] NULL,
	[OlcuBirimi] [nvarchar](10) NULL,
	[Status] [bit] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Category2Id] [int] NULL,
	[Category3Id] [int] NULL,
	[Category4Id] [int] NULL,
	[VendorId] [int] NOT NULL,
	[Views] [int] NOT NULL,
	[Keywords] [nvarchar](250) NULL,
	[CDate] [datetime] NULL,
	[GercekStokKodu] [nvarchar](250) NULL,
	[UrunSayisi] [int] NULL,
	[OzelKod1] [nvarchar](100) NULL,
	[url] [nvarchar](250) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductViews]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductViews](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[Tarayici] [nvarchar](max) NULL,
	[Isim] [nvarchar](max) NULL,
	[Versiyon] [nvarchar](max) NULL,
	[IsletimSistemi] [nvarchar](max) NULL,
	[IP] [nvarchar](max) NULL,
	[Aciklama1] [nvarchar](max) NULL,
	[Aciklama2] [nvarchar](max) NULL,
	[CDate] [datetime] NULL,
 CONSTRAINT [PK_ProductViews] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Review]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[VendorId] [int] NOT NULL,
	[Detail] [ntext] NULL,
	[DatePost] [date] NOT NULL,
 CONSTRAINT [PK_RatingReview] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sehirler]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sehirler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Isim] [nvarchar](100) NULL,
	[Kod] [nvarchar](50) NULL,
 CONSTRAINT [PK_Sehirler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](250) NULL,
	[Value] [ntext] NULL,
	[Group] [varchar](250) NULL,
	[TypeOfControl] [varchar](50) NULL,
	[CDate] [datetime] NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendor]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](250) NULL,
	[Password] [varchar](250) NULL,
	[Name] [varchar](250) NULL,
	[Address] [varchar](250) NULL,
	[Email] [varchar](250) NULL,
	[Phone] [varchar](50) NULL,
	[Logo] [varchar](50) NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Vendor] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[YanlisKelimeler]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[YanlisKelimeler](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Yanlis] [nvarchar](100) NULL,
	[Dogru] [nvarchar](100) NULL,
 CONSTRAINT [PK_YanlisKelimeler] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[SiteHaritasi]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



 CREATE VIEW [dbo].[SiteHaritasi]
 AS
SELECT TOP (9999999) 
	    --required loc
	    '<url><loc>https://www.ciftteker.com/' 
            + url
			+ '</loc><lastmod>'+CONVERT(VARCHAR(10), GETDATE(), 20)+'</lastmod><priority>1</priority></url>'
			 AS "loc"
    FROM dbo.Product P 
	WHERE P.Status = 1
    ORDER BY P.Id

GO
/****** Object:  View [dbo].[SiteHaritasiDetayli]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[SiteHaritasiDetayli]
 AS
SELECT TOP (9999999) 
	    --required loc
	    '<url><loc>https://www.ciftteker.com/' 
            + p.url
			+ '</loc><lastmod>'+CONVERT(VARCHAR(10), GETDATE(), 20)+'</lastmod><priority>1</priority>'
			+ '<unite>'+P.OlcuBirimi+'</unite>'
			+ '<category1>'+K1.Name+'</category1>'
			+ '<category2>'+K2.Name+'</category2>'
			+ '<category3>'+K3.Name+'</category3>'
			+ '<code>'+P.Code+'</code>'
			+ '<title>'+P.Name+'</title>'
			+ '<price>'+CAST(P.Price AS NVARCHAR(MAX))+'</price>'
			+ '<description>'+CAST(P.Description AS NVARCHAR(MAX))+'</description>'
			+ '<image>'+ISNULL((SELECT TOP(1) 'https://ozerdemmotosiklet.com/Resimler/'+PH.Name FROM dbo.Photo PH(NOLOCK) WHERE PH.ProductId = P.ID AND PH.Main = 1 AND PH.Status = 1),'https://ozerdemmotosiklet.com/Resimler/resimyok.png')+'</image>'
			+ '</url>'
			 AS "loc"
    FROM dbo.Product P 
	INNER JOIN dbo.Category K1 ON P.CategoryId = K1.Id
	INNER JOIN dbo.Category K2 ON P.Category2Id = K2.Id
	INNER JOIN dbo.Category K3 ON P.Category3Id = K3.Id
	WHERE P.Status = 1
    ORDER BY P.Id


GO
/****** Object:  View [dbo].[vw_Aktarim_Stoklar]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Aktarim_Stoklar]
as
Select 0 BaglantiID, Code StokKodu from dbo.Product(NOLOCK)


GO
ALTER TABLE [dbo].[AspNet_SqlCacheTablesForChangeNotification] ADD  DEFAULT (getdate()) FOR [notificationCreated]
GO
ALTER TABLE [dbo].[AspNet_SqlCacheTablesForChangeNotification] ADD  DEFAULT ((0)) FOR [changeId]
GO
ALTER TABLE [dbo].[Bankalar] ADD  CONSTRAINT [DF_Bankalar_Aktif]  DEFAULT ((1)) FOR [Aktif]
GO
ALTER TABLE [dbo].[BankaXml] ADD  CONSTRAINT [DF_BankaXml_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[Card] ADD  CONSTRAINT [DF_Card_Quantity]  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Card] ADD  CONSTRAINT [DF_Card_Aktarilacak]  DEFAULT ((0)) FOR [Aktarilacak]
GO
ALTER TABLE [dbo].[CVV] ADD  CONSTRAINT [DF_CVV_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[Loglar] ADD  CONSTRAINT [DF_Loglar_Tarih]  DEFAULT (getdate()) FOR [Tarih]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_Aktarildi]  DEFAULT ((0)) FOR [Aktarildi]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_Ertele]  DEFAULT ((0)) FOR [Ertele]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_Aktif]  DEFAULT ((0)) FOR [Aktif]
GO
ALTER TABLE [dbo].[OrdersDetailLog] ADD  CONSTRAINT [DF_OrdersDetailLog_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[OrdersLog] ADD  CONSTRAINT [DF_OrdersLog_Aktarildi]  DEFAULT ((0)) FOR [Aktarildi]
GO
ALTER TABLE [dbo].[OrdersLog] ADD  CONSTRAINT [DF_OrdersLog_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[OrdersLog] ADD  CONSTRAINT [DF_OrdersLog_Ertele]  DEFAULT ((0)) FOR [Ertele]
GO
ALTER TABLE [dbo].[PosOdemeleri] ADD  CONSTRAINT [DF_PosOdemeleri_Tip]  DEFAULT (N'Sonuc') FOR [Tip]
GO
ALTER TABLE [dbo].[PosOdemeleri] ADD  CONSTRAINT [DF_PosOdemeleri_GonderilenTutar]  DEFAULT ((0)) FOR [GonderilenTutar]
GO
ALTER TABLE [dbo].[PosOdemeleri] ADD  CONSTRAINT [DF_PosOdemeleri_KayitTarihi]  DEFAULT (getdate()) FOR [KayitTarihi]
GO
ALTER TABLE [dbo].[PosOdemeleri] ADD  CONSTRAINT [DF_PosOdemeleri_KontrolEdildi]  DEFAULT ((0)) FOR [KontrolEdildi]
GO
ALTER TABLE [dbo].[PosOdemeleri] ADD  CONSTRAINT [DF_PosOdemeleri_Aktarildi]  DEFAULT ((0)) FOR [Aktarildi]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[ProductViews] ADD  CONSTRAINT [DF_ProductViews_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[Setting] ADD  CONSTRAINT [DF_Setting_CDate]  DEFAULT (getdate()) FOR [CDate]
GO
ALTER TABLE [dbo].[Adresler]  WITH CHECK ADD  CONSTRAINT [FK_Adresler_Account] FOREIGN KEY([UserID])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Adresler] CHECK CONSTRAINT [FK_Adresler_Account]
GO
ALTER TABLE [dbo].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_Account] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Card] CHECK CONSTRAINT [FK_Card_Account]
GO
ALTER TABLE [dbo].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Card] CHECK CONSTRAINT [FK_Card_Product]
GO
ALTER TABLE [dbo].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[Card] CHECK CONSTRAINT [FK_Card_Vendor]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Category] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Category]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Vendor]
GO
ALTER TABLE [dbo].[HomeProducts]  WITH CHECK ADD  CONSTRAINT [FK_HomeProducts_Product] FOREIGN KEY([ProdutID])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[HomeProducts] CHECK CONSTRAINT [FK_HomeProducts_Product]
GO
ALTER TABLE [dbo].[MemberShipVendor]  WITH CHECK ADD  CONSTRAINT [FK_MemberShipVendor_MemberShip] FOREIGN KEY([MemerShipId])
REFERENCES [dbo].[MemberShip] ([Id])
GO
ALTER TABLE [dbo].[MemberShipVendor] CHECK CONSTRAINT [FK_MemberShipVendor_MemberShip]
GO
ALTER TABLE [dbo].[MemberShipVendor]  WITH CHECK ADD  CONSTRAINT [FK_MemberShipVendor_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[MemberShipVendor] CHECK CONSTRAINT [FK_MemberShipVendor_Vendor]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Account] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Account]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Vendor]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Account] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Account]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderStatus] FOREIGN KEY([OrderStatusId])
REFERENCES [dbo].[OrderStatus] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderStatus]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Payment]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Vendor]
GO
ALTER TABLE [dbo].[OrdersDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetail_Orders] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
GO
ALTER TABLE [dbo].[OrdersDetail] CHECK CONSTRAINT [FK_OrdersDetail_Orders]
GO
ALTER TABLE [dbo].[OrdersDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetail_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrdersDetail] CHECK CONSTRAINT [FK_OrdersDetail_Product]
GO
ALTER TABLE [dbo].[OrdersDetailLog]  WITH CHECK ADD  CONSTRAINT [FK_OrdersDetailLog_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrdersDetailLog] CHECK CONSTRAINT [FK_OrdersDetailLog_Product]
GO
ALTER TABLE [dbo].[OrdersLog]  WITH CHECK ADD  CONSTRAINT [FK_OrdersLog_Account] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[OrdersLog] CHECK CONSTRAINT [FK_OrdersLog_Account]
GO
ALTER TABLE [dbo].[OrdersLog]  WITH CHECK ADD  CONSTRAINT [FK_OrdersLog_Orderstatus] FOREIGN KEY([OrderStatusId])
REFERENCES [dbo].[OrderStatus] ([Id])
GO
ALTER TABLE [dbo].[OrdersLog] CHECK CONSTRAINT [FK_OrdersLog_Orderstatus]
GO
ALTER TABLE [dbo].[OrdersLog]  WITH CHECK ADD  CONSTRAINT [FK_OrdersLog_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[OrdersLog] CHECK CONSTRAINT [FK_OrdersLog_Payment]
GO
ALTER TABLE [dbo].[OrdersLog]  WITH CHECK ADD  CONSTRAINT [FK_OrdersLog_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[OrdersLog] CHECK CONSTRAINT [FK_OrdersLog_Vendor]
GO
ALTER TABLE [dbo].[Photo]  WITH CHECK ADD  CONSTRAINT [FK_Photo_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Photo] CHECK CONSTRAINT [FK_Photo_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category1] FOREIGN KEY([Category2Id])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category1]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category2] FOREIGN KEY([Category3Id])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category2]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category3] FOREIGN KEY([Category4Id])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category3]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Vendor]
GO
ALTER TABLE [dbo].[ProductViews]  WITH CHECK ADD  CONSTRAINT [FK_ProductViews_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[ProductViews] CHECK CONSTRAINT [FK_ProductViews_Product]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_Account] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_Account]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([Id])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_Vendor]
GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCachePollingStoredProcedure]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCachePollingStoredProcedure] AS
         SELECT tableName, changeId FROM dbo.AspNet_SqlCacheTablesForChangeNotification
         RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheQueryRegisteredTablesStoredProcedure]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheQueryRegisteredTablesStoredProcedure] 
         AS
         SELECT tableName FROM dbo.AspNet_SqlCacheTablesForChangeNotification   
GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheRegisterTableStoredProcedure]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheRegisterTableStoredProcedure] 
             @tableName NVARCHAR(450) 
         AS
         BEGIN

         DECLARE @triggerName AS NVARCHAR(3000) 
         DECLARE @fullTriggerName AS NVARCHAR(3000)
         DECLARE @canonTableName NVARCHAR(3000) 
         DECLARE @quotedTableName NVARCHAR(3000) 

         /* Create the trigger name */ 
         SET @triggerName = REPLACE(@tableName, '[', '__o__') 
         SET @triggerName = REPLACE(@triggerName, ']', '__c__') 
         SET @triggerName = @triggerName + '_AspNet_SqlCacheNotification_Trigger' 
         SET @fullTriggerName = 'dbo.[' + @triggerName + ']' 

         /* Create the cannonicalized table name for trigger creation */ 
         /* Do not touch it if the name contains other delimiters */ 
         IF (CHARINDEX('.', @tableName) <> 0 OR 
             CHARINDEX('[', @tableName) <> 0 OR 
             CHARINDEX(']', @tableName) <> 0) 
             SET @canonTableName = @tableName 
         ELSE 
             SET @canonTableName = '[' + @tableName + ']' 

         /* First make sure the table exists */ 
         IF (SELECT OBJECT_ID(@tableName, 'U')) IS NULL 
         BEGIN 
             RAISERROR ('00000001', 16, 1) 
             RETURN 
         END 

         BEGIN TRAN
         /* Insert the value into the notification table */ 
         IF NOT EXISTS (SELECT tableName FROM dbo.AspNet_SqlCacheTablesForChangeNotification WITH (NOLOCK) WHERE tableName = @tableName) 
             IF NOT EXISTS (SELECT tableName FROM dbo.AspNet_SqlCacheTablesForChangeNotification WITH (TABLOCKX) WHERE tableName = @tableName) 
                 INSERT  dbo.AspNet_SqlCacheTablesForChangeNotification 
                 VALUES (@tableName, GETDATE(), 0)

         /* Create the trigger */ 
         SET @quotedTableName = QUOTENAME(@tableName, '''') 
         IF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = 'TR') 
             IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = 'TR') 
                 EXEC('CREATE TRIGGER ' + @fullTriggerName + ' ON ' + @canonTableName +'
                       FOR INSERT, UPDATE, DELETE AS BEGIN
                       SET NOCOUNT ON
                       EXEC dbo.AspNet_SqlCacheUpdateChangeIdStoredProcedure N' + @quotedTableName + '
                       END
                       ')
         COMMIT TRAN
         END
   
GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheUnRegisterTableStoredProcedure]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheUnRegisterTableStoredProcedure] 
             @tableName NVARCHAR(450) 
         AS
         BEGIN

         BEGIN TRAN
         DECLARE @triggerName AS NVARCHAR(3000) 
         DECLARE @fullTriggerName AS NVARCHAR(3000)
         SET @triggerName = REPLACE(@tableName, '[', '__o__') 
         SET @triggerName = REPLACE(@triggerName, ']', '__c__') 
         SET @triggerName = @triggerName + '_AspNet_SqlCacheNotification_Trigger' 
         SET @fullTriggerName = 'dbo.[' + @triggerName + ']' 

         /* Remove the table-row from the notification table */ 
         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = 'AspNet_SqlCacheTablesForChangeNotification' AND type = 'U') 
             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = 'AspNet_SqlCacheTablesForChangeNotification' AND type = 'U') 
             DELETE FROM dbo.AspNet_SqlCacheTablesForChangeNotification WHERE tableName = @tableName 

         /* Remove the trigger */ 
         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = 'TR') 
             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = 'TR') 
             EXEC('DROP TRIGGER ' + @fullTriggerName) 

         COMMIT TRAN
         END
   
GO
/****** Object:  StoredProcedure [dbo].[AspNet_SqlCacheUpdateChangeIdStoredProcedure]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AspNet_SqlCacheUpdateChangeIdStoredProcedure] 
             @tableName NVARCHAR(450) 
         AS

         BEGIN 
             UPDATE dbo.AspNet_SqlCacheTablesForChangeNotification WITH (ROWLOCK) SET changeId = changeId + 1 
             WHERE tableName = @tableName
         END
   
GO
/****** Object:  StoredProcedure [dbo].[IDP_SiteHaritasi]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[IDP_SiteHaritasi](
@Sira1 int,
@sira2 int
)
AS
BEGIN
 SELECT 0 Sira,'<?xml version="1.0" encoding="UTF-8"?>' AS loc
 UNION all
 SELECT 0, '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"> ' AS loc
 UNION all
SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN @Sira1 AND @Sira2		

 UNION all
 SELECT 0, '</urlset>' AS loc
END
GO
/****** Object:  StoredProcedure [dbo].[IDP_SiteHaritasiDetayli]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[IDP_SiteHaritasiDetayli]
AS
BEGIN
 SELECT 0 Sira,'<?xml version="1.0" encoding="UTF-8"?>' AS loc
 UNION all
 SELECT 0, '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"> ' AS loc
 UNION all
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 1 AND 1000			  --1
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 1001 AND 2000		  --2
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 2001 AND 3000		  --3
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 3001 AND 4000		  --4
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 4001 AND 5000		  --5
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 5001 AND 6000		  --6
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 6001 AND 7000		  --7
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 7001 AND 8000		  --8
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 8001 AND 9000		  --9
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 9001 AND 10000		  --10
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 10001 AND 11000		  --11
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 11001 AND 12000		  --12
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 12001 AND 13000		  --13
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 13001 AND 14000		  --14
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 14001 AND 15000		  --15
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 15001 AND 16000		  --16
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 16001 AND 17000		  --17
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 17001 AND 18000		  --18
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 18001 AND 19000		  --19
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 19001 AND 20000		  --20
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 20001 AND 21000		  --21
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 21001 AND 22000		  --22
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 22001 AND 23000		  --23
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 23001 AND 24000		  --24
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 24001 AND 25000		  --25
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 25001 AND 26000		  --26
--SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasi ORDER BY loc ASC) R1 WHERE Sira BETWEEN 26001 AND 27000		  --27
SELECT * FROM (SELECT TOP(9999999) ROW_NUMBER() OVER (ORDER BY loc asc) Sira,* FROM dbo.SiteHaritasiDetayli ORDER BY loc ASC) R1                              		  -- Hepsi
 UNION all
 SELECT 0, '</urlset>' AS loc
END
GO
/****** Object:  StoredProcedure [dbo].[p_AktarilacakSiparislerETicaret]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[p_AktarilacakSiparislerETicaret]
AS
    BEGIN

        SELECT  Sip.ID ,
                Sip.DateCreation AS Tarih ,
				'ETIC'+RIGHT('0000000000000'+CAST(CASE WHEN Sip.ID =  18434 THEN '99'+CAST(Sip.ID as nvarchar(max)) ELSE Sip.ID END AS NVARCHAR(100)),11) AS BelgeNo,
                S.Code AS StokKodu ,
                S.Code AS YedekStokKodu ,
                'ETIC'+CAST(CASE WHEN A.Id = 1 THEN 'S'+CAST(Sip.Id AS NVARCHAR(MAX)) ELSE CAST(A.Id AS NVARCHAR(MAX)) END AS NVARCHAR(MAX)) AS CariKodu ,
				UPPER(CASE WHEN Sip.FaturaTuru = 'K' THEN Sip.FirmaAdi ELSE (CASE WHEN A.FullName = 'YUNUS KÖSE' THEN Sip.Isim ELSE A.FullName END) END) CariAdi,
				UPPER(CASE WHEN Sip.FaturaTuru = 'K' THEN Sip.FirmaAdi ELSE Sip.Isim END) + ' - ' + Sip.CepTelefonu TeslimatAdi,
				dbo.UpperFirstCharacter(REPLACE(Sip.Adres,'''','-')) CariAdres,
				CASE WHEN A.Email = 'info@idyazilim.com' THEN ISNULL(Sip.Email,A.Email) ELSE ISNULL(A.Email,Sip.Email) END CariEMail,
			    CASE WHEN A.Phone = '05355089134' THEN Sip.CepTelefonu ELSE A.Phone END CariTelefon,
				Seh.Isim CariIl,
				Sip.Ilce CariIlce,
				Sip.VergiDairesi,
				Sip.VergiNumarasi,
				Sip.TC,
                SipD.Quantity AS Miktar ,
                18 AS Kdv ,
                'AD' AS Birimi ,
				SipD.Price AS Fiyat ,
                SipD.Quantity * SipD.Price AS Tutar ,
                '' AS Notlar ,
                Sip.DateCreation as KayitTarihi ,
                'ETICARET' AS KayitYapanKullanici ,
                Sip.Aktarildi ,
                '00' PlasiyerKodu,
				'' Hata
        FROM    dbo.Orders Sip (NOLOCK)
				INNER JOIN dbo.OrdersDetail SipD (NOLOCK) ON Sip.Id = SipD.OrderId
                LEFT JOIN dbo.Product S ( NOLOCK ) ON SipD.ProductId = S.Id
                LEFT JOIN Account A ( NOLOCK ) ON Sip.CustomerId = A.Id
				LEFT JOIN dbo.Sehirler Seh (NOLOCK) ON Seh.Kod = Sip.Il
        WHERE   1=1 
		        and S.Status = 1
		        AND ISNULL(Sip.Aktarildi,0) = 0
				AND S.Code IS NOT NULL
				AND Sip.CDate <= DATEADD(MINUTE,4,GETDATE())
				AND ISNULL(Sip.Ertele,0) <> 1
				AND Sip.Aktif = 1
				
ORDER BY        Sip.CDate DESC


Update Photo set Main = 0 
--select * from Photo
Where Id IN (
select MAX(Photo.Id) from Photo Where Main = 1
group by ProductId,main
having count(*) > 1
)


    END
GO
/****** Object:  StoredProcedure [dbo].[p_AktarimStokKaydet]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[p_AktarimStokKaydet](
@BaglantiID int=0,
@StokKodu nvarchar(max)='',
@StokAdi nvarchar(max)='',
@Barkod nvarchar(max)='',
@Barkod2 nvarchar(max)='',
@Barkod3 nvarchar(max)='',
@Kategori1 nvarchar(max)='',
@Kategori2 nvarchar(max)='',
@Kategori3 nvarchar(max)='',
@Kategori4 nvarchar(max)='',
@Bakiye decimal(18,2)=0,
@Kdv decimal(18,2)=0,
@Birimi nvarchar(max)='',
@Aktiflik bit=1,
@Fiyat1 decimal(18,2)=0,
@Bilgi nvarchar(max)='',
@GercekStokKodu nvarchar(max)='',
@OzelKod1 NVARCHAR(MAX)='',
@Aciklama NVARCHAR(MAX)=''
)
as
BEGIn
	
	Insert Into Loglar (Tip,Kullanici,Tarih,Aciklama) values ('UrunAktarimi-ETicaret',@StokKodu,GETDATE(),@Fiyat1)

	set @StokKodu = (@StokKodu)
	set @Barkod = (@Barkod)

	IF (@StokAdi NOT LIKE '%****%')
	BEGIN

	IF EXISTS(Select * from dbo.Product S(NOLOCK) Where S.Code = @StokKodu)
	BEGIN
	PRINT('Update')
		Update Product Set 
			Code		=@StokKodu,
			[Name]		=@StokAdi,
			Price		=@Fiyat1,
			[Description] = @StokAdi+'<br/>'+@Aciklama,
			[Status]		=@Aktiflik,
			Keywords	=@Barkod + REPLACE(@StokAdi,' ',', '),
			--[Views]			=0,
			VendorId		=18,
			Quantity			=@Bakiye,
			CategoryId		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori1 AND dbo.Category.Status = 1 AND dbo.Category.ParentId IS NULL),
			Category2Id		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori2 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL)),
			Category3Id		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori3 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C1.Id FROM dbo.Category C1 (NOLOCK) WHERE C1.Name = @Kategori2 AND C1.Status = 1 AND C1.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL))  ),
			Category4Id		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori4 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori3 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C1.Id FROM dbo.Category C1 (NOLOCK) WHERE C1.Name = @Kategori2 AND C1.Status = 1 AND C1.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL))  )),
			GercekStokKodu = @GercekStokKodu,
			OlcuBirimi = @Birimi,
			OzelKod1 = @OzelKod1--,
			--url = REPLACE(REPLACE(dbo.Temizle(@StokAdi+'-'+@StokKodu),' ','-'),'.','-')
		Where Code = @StokKodu
		
		/*
		Update [spormotor.com].dbo.Product Set 
			Code		=@StokKodu,
			[Name]		=@StokAdi,
			Price		=@Fiyat1,
			[Description] = @StokAdi+'<br/>'+@Aciklama,
			[Status]		=@Aktiflik,
			Keywords	=@Barkod + REPLACE(@StokAdi,' ',', '),
			--[Views]			=0,
			VendorId		=18,
			Quantity			=@Bakiye,
			CategoryId		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori1 AND dbo.Category.Status = 1 AND dbo.Category.ParentId IS NULL),
			Category2Id		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori2 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL)),
			Category3Id		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori3 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C1.Id FROM dbo.Category C1 (NOLOCK) WHERE C1.Name = @Kategori2 AND C1.Status = 1 AND C1.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL))  ),
			Category4Id		=(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori4 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori3 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C1.Id FROM dbo.Category C1 (NOLOCK) WHERE C1.Name = @Kategori2 AND C1.Status = 1 AND C1.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL))  )),
			GercekStokKodu = @GercekStokKodu,
			OlcuBirimi = @Birimi,
			OzelKod1 = @OzelKod1--,
			--url = REPLACE(REPLACE(dbo.Temizle(@StokAdi+'-'+@StokKodu),' ','-'),'.','-')
		Where Code = @StokKodu
		*/
	END
	ELSE
	BEGIN
		Insert Into Product 
		(Code,[Name],Price,[Description],[Status],Keywords,[Views],VendorId,Quantity,CategoryId,Category2Id,Category3Id,GercekStokKodu,OlcuBirimi,OzelKod1,url)
		Select 
		@StokKodu,@StokAdi,@Fiyat1,@StokAdi+'<br/>'+@Aciklama,@Aktiflik,@Barkod + REPLACE(@StokAdi,' ',', '),0,18,@Bakiye,
		(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori1 AND dbo.Category.Status = 1 AND dbo.Category.ParentId IS NULL),
		(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori2 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL)),
		(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori3 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C1.Id FROM dbo.Category C1 (NOLOCK) WHERE C1.Name = @Kategori2 AND C1.Status = 1 AND C1.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL))  ),
		@GercekStokKodu,
		@Birimi,
		@OzelKod1,
		REPLACE(REPLACE(dbo.Temizle(@StokAdi+'-'+@StokKodu),' ','-'),'.','-')
		WHERE (SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori1 AND dbo.Category.Status = 1 AND dbo.Category.ParentId IS NULL) IS NOT NULL

		/*
		Insert Into [spormotor.com].dbo.Product 
		(Code,[Name],Price,[Description],[Status],Keywords,[Views],VendorId,Quantity,CategoryId,Category2Id,Category3Id,GercekStokKodu,OlcuBirimi,OzelKod1,url)
		Select 
		@StokKodu,@StokAdi,@Fiyat1,@StokAdi+'<br/>'+@Aciklama,@Aktiflik,@Barkod + REPLACE(@StokAdi,' ',', '),0,18,@Bakiye,
		(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori1 AND dbo.Category.Status = 1 AND dbo.Category.ParentId IS NULL),
		(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori2 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL)),
		(SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori3 AND dbo.Category.Status = 1 AND dbo.Category.ParentId  = (SELECT C1.Id FROM dbo.Category C1 (NOLOCK) WHERE C1.Name = @Kategori2 AND C1.Status = 1 AND C1.ParentId  = (SELECT C.Id FROM dbo.Category C (NOLOCK) WHERE C.Name = @Kategori1 AND C.Status = 1 AND C.ParentId IS NULL))  ),
		@GercekStokKodu,
		@Birimi,
		@OzelKod1,
		REPLACE(REPLACE(dbo.Temizle(@StokAdi+'-'+@StokKodu),' ','-'),'.','-')
		WHERE (SELECT Id FROM dbo.Category (NOLOCK) WHERE dbo.Category.Name = @Kategori1 AND dbo.Category.Status = 1 AND dbo.Category.ParentId IS NULL) IS NOT NULL
		*/

	END
	END

END


GO
/****** Object:  StoredProcedure [dbo].[p_BilgileriGuncelle]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[p_BilgileriGuncelle]
AS
BEGIN

Insert Into Loglar (Tip,Kullanici,Tarih,Aciklama) values ('p_BilgileriGuncelle','IDYAZILIM',GETDATE(),'Otomatik Çalıştı')

PRINT(CAST(GETDATE() AS TIME))
--Resmi olmayan ürünler için resim otomatik insert kodu 
INSERT INTO OzerdemETicaret.dbo.Photo
(
    Name,
Status,
    Main,
    ProductId
)
SELECT RR.ResimYolu,1,1,P.Id FROM OzerdemETicaret.dbo.Product P WITH(NOLOCK) 
INNER JOIN OzerdemB2B.dbo.Resimler RR WITH(NOLOCK,INDEX(IX_Resimler_StokKodu,IX_Resimler_ResimYolu)) ON RR.StokKodu = P.Code
LEFT OUTER JOIN OzerdemETicaret.dbo.Product P1 WITH(NOLOCK) ON P1.Name = RR.ResimYolu
WHERE 1=1 --AND P.Code = 'Y4MON0240A0121'
AND (
(SELECT COUNT(1) FROM OzerdemETicaret.dbo.Photo PP WITH(NOLOCK,INDEX(IX_Photo)) WHERE PP.ProductId = P.Id) 
<>
(SELECT COUNT(1) FROM OzerdemB2B.dbo.Resimler R WITH(NOLOCK,INDEX(IX_Resimler_StokKodu)) WHERE R.StokKodu = P.Code)
)
AND RR.ResimYolu <> ''
and P1.Id IS NULL
--AND RR.ResimYolu NOT IN (SELECT PP1.[Name] FROM OzerdemETicaret.dbo.Photo PP1 WITH(NOLOCK))

PRINT(CAST(GETDATE() AS TIME))

-- Resimlerin otomatik stok kodunu güncelleme olayı.
-- BURADAKİ TOP 1000 KALDIRILIRSA RESİMLERİN HEPSİNİ UPDATE EDER
UPDATE  OzerdemB2B.dbo.Resimler 
SET StokKodu = (SELECT Stoklar.StokKodu FROM OzerdemB2B.dbo.Stoklar WHERE Stoklar.ID = Resimler.StokID)
--WHERE StokKodu IS NULL
--AND (SELECT COUNT(*) FROM OzerdemB2B.dbo.Stoklar WHERE Stoklar.ID = Resimler.StokID) > 0

PRINT(CAST(GETDATE() AS TIME))

--Pasif Ürünleri Silme
Update OzerdemETicaret.dbo.Product SET Status = 0 WHERE Status = 1 and Code NOT IN (SELECT StokKodu FROM OzerdemB2B.dbo.Stoklar WITH(NOLOCK))

PRINT(CAST(GETDATE() AS TIME))

-- Silinen ürün resimleri için 
delete FROM OzerdemB2B.dbo.Resimler WHERE StokID NOT IN (SELECT S.ID FROM OzerdemB2B.dbo.Stoklar AS S (NOLOCK)) AND StokID > 0

PRINT(CAST(GETDATE() AS TIME))

DELETE from dbo.Photo WHERE Name NOT IN (SELECT ResimYolu FROM OzerdemB2B.dbo.Resimler (NOLOCK) WHERE StokID > 0)

PRINT(CAST(GETDATE() AS TIME))

UPDATE OzerdemETicaret.dbo.Photo SET Main = 1 WHERE Main = 0
AND (SELECT COUNT(*) FROM dbo.Photo P(NOLOCK) WHERE P.ProductId = dbo.Photo.ProductId  ) = 1

PRINT(CAST(GETDATE() AS TIME))

END
GO
/****** Object:  StoredProcedure [dbo].[p_StokAra]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[p_StokAra](
@Ara NVARCHAR(MAX)='',
@Kategori1 NVARCHAR(MAX)='',
@Kategori2 NVARCHAR(MAX)='',
@Kategori3 nvarchar(MAX)='',
@Kategori4 nvarchar(MAX)='',
@Sayfa int
)
AS
BEGIN

	DECLARE @Ara2 nvarchar(250)= REPLACE(@Ara,' ',',')
	
	SET @Ara =REPLACE(@Ara,' ','%')
	

	IF @Ara IS NULL BEGIN SET @Ara = '' END
	
	CREATE TABLE #IDLer(ID INT,Sira INT); 
	IF @Kategori1 = '0'
	BEGIN
INSERT INTO #IDLer(ID,Sira)
SELECT  
	P.Id,ROW_NUMBER() OVER (ORDER BY COUNT(*) desc,P.Id)
	FROM dbo.Product P WITH(NOLOCK)
	CROSS APPLY dbo.StringArray(@Ara2,',') s
	WHERE 1=1
	AND P.Status = 1 
	AND (P.Code LIKE N'%'+@Ara+'%' OR P.Name LIKE N'%'+s.FieldValue+'%' or P.Description like  N'%'+@Ara+'%')
	GROUP BY 
	P.Id
	ORDER BY COUNT(*) desc, P.Id
	END
	ELSE IF @Kategori2 = '0'
	BEGIN
	INSERT INTO #IDLer(ID,Sira)
SELECT  
	P.Id,ROW_NUMBER() OVER (ORDER BY COUNT(*) desc,P.Id)
	FROM dbo.Product P WITH(NOLOCK)
	LEFT JOIN dbo.Category C1 WITH(NOLOCK) ON C1.Id = P.CategoryId
	CROSS APPLY dbo.StringArray(@Ara2,',') s
	WHERE 1=1
	AND P.Status = 1 
	AND (P.Code LIKE N'%'+@Ara+'%' OR P.Name LIKE N'%'+s.FieldValue+'%' or P.Description like  N'%'+@Ara+'%')
	AND C1.Id IS NOT NULL
	AND (C1.Id = @Kategori1) 
	GROUP BY 
	P.Id
	ORDER BY COUNT(*) desc, P.Id
	END
	ELSE
	BEGIN
INSERT INTO #IDLer(ID,Sira)
SELECT  
	P.Id,ROW_NUMBER() OVER (ORDER BY COUNT(*) desc,P.Id)
	FROM dbo.Product P WITH(NOLOCK)
	LEFT JOIN dbo.Category C1 WITH(NOLOCK) ON C1.Id = P.CategoryId
	LEFT JOIN dbo.Category C2 WITH(NOLOCK) ON C2.Id = P.Category2Id
	LEFT JOIN dbo.Category C3 WITH(NOLOCK) ON C3.Id = P.Category3Id
	LEFT JOIN dbo.Category C4 WITH(NOLOCK) ON C4.Id = P.Category4Id
	CROSS APPLY dbo.StringArray(@Ara2,',') s
	WHERE 1=1
	AND P.Status = 1 
	AND C1.Id IS NOT NULL
	AND C2.Id IS NOT NULL
	--AND C3.Id IS NOT NULL
	--AND C4.Id IS NOT NULL
	AND (C1.Id = @Kategori1 OR @Kategori1 = '0') 
	AND (C2.Id = @Kategori2 OR @Kategori2 = '0') 
	AND (C3.Id = @Kategori3 OR @Kategori3 = '0') 
	AND (C4.Id = @Kategori4 OR @Kategori4 = '0') 
	--AND (P.Code LIKE N'%'+@Ara+'%' OR P.Name LIKE N'%'+@Ara+'%' OR C1.Name LIKE N'%'+@Ara+'%' OR C2.Name LIKE N'%'+@Ara+'%'OR C3.Name LIKE N'%'+@Ara+'%')
	AND (P.Code LIKE N'%'+@Ara+'%' OR P.Name LIKE N'%'+s.FieldValue+'%' or P.Description like  N'%'+@Ara+'%')
	GROUP BY 
	P.Id
	ORDER BY COUNT(*) desc, P.Id
	END

	DECLARE @UrunSayisi INT  = (SELECT count(*) FROM #IDLer WITH(NOLOCK) )


	SELECT  
	P.Id,
	P.Code,
	P.[Name],
	P.Price,
	CAST(P.[Description] AS NVARCHAR(MAX)) AS [Description],
	P.OlcuBirimi,
	P.[Status],
	P.CategoryId,
	P.Category2Id,
	P.Category3Id,
	P.Category4Id,
	P.VendorId,
	P.[Views],
	P.Keywords,
	P.CDate,
	P.GercekStokKodu,
	P.Quantity,
	@UrunSayisi UrunSayisi,
	P.OzelKod1,
	P.url
	--,count(*)
	FROM dbo.Product P WITH(NOLOCK)
	INNER JOIN #IDLer I WITH(NOLOCK) ON P.Id = I.ID
	--Where P.Id IN (SELECT I.ID FROM #IDLer as I WITH(NOLOCK) )
	--ORDER BY COUNT(*) desc, P.Id
	ORDER BY I.Sira
	OFFSET (@Sayfa-1)*20 ROWS
	FETCH NEXT 20 ROWS ONLY;

	DROP TABLE #IDLer;

END

GO
/****** Object:  StoredProcedure [dbo].[p_StokSil]    Script Date: 07.02.2023 00:21:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[p_StokSil](
@StokKodu nvarchar(max)
)
as
BEGIN
--Insert Into Deneme (Deger) values (@StokKodu)
UPDATE Product SET Status = 0 WHERE Code = @StokKodu
END


GO
