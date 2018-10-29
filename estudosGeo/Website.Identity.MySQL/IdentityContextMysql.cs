﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;

namespace Website.Identity.MySQL
{
    /// <summary>
    /// Default IdentityDbContext that uses the default entity types for ASP.NET Identity Users,
    /// Roles, Claims, Logins. Use this overload to add your own entity types.
    /// </summary>
    public class IdentityContextMysql :
        IdentityContextMysql<IdentityUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        /// <summary>
        /// Default constructor which uses the DefaultConnection
        /// </summary>
        public IdentityContextMysql()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public IdentityContextMysql(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model. The connection will not be disposed
        /// when the context is disposed if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model. The connection will not be disposed
        /// when the context is disposed if contextOwnsConnection is false.
        /// </param>
        public IdentityContextMysql(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of the database
        /// to which a connection will be made, and initializes it from the given model. The
        /// by-convention name is the full name (namespace + class
        /// name) of the derived context class. See the class remarks for how this is used to create
        ///       a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        public IdentityContextMysql(DbCompiledModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a
        /// database. The connection will not be disposed when the context is disposed if
        /// contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">
        /// If set to true the connection is disposed when the context is disposed, otherwise the
        /// caller must dispose the connection.
        /// </param>
        public IdentityContextMysql(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes it from the
        /// given model. See the class remarks for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public IdentityContextMysql(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }
    }

    /// <summary>
    /// DbContext which uses a custom user entity with a string primary key
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class IdentityContextMysql<TUser> : IdentityContextMysql<TUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
        where TUser : IdentityUser
    {
        /// <summary>
        /// Default constructor which uses the DefaultConnection
        /// </summary>
        public IdentityContextMysql()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public IdentityContextMysql(string nameOrConnectionString)
            : this(nameOrConnectionString, true)
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        /// <param name="throwIfV1Schema">
        /// Will throw an exception if the schema matches that of Identity 1.0.0
        /// </param>
        public IdentityContextMysql(string nameOrConnectionString, bool throwIfV1Schema)
            : base(nameOrConnectionString)
        {
            if (throwIfV1Schema && IsIdentityV1Schema(this))
            {
                throw new InvalidOperationException(IdentityResources.IdentityV1SchemaError);
            }
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model. The connection will not be disposed
        /// when the context is disposed if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model. The connection will not be disposed
        /// when the context is disposed if contextOwnsConnection is false.
        /// </param>
        public IdentityContextMysql(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of the database
        /// to which a connection will be made, and initializes it from the given model. The
        /// by-convention name is the full name (namespace + class
        /// name) of the derived context class. See the class remarks for how this is used to create
        ///       a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        public IdentityContextMysql(DbCompiledModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a
        /// database. The connection will not be disposed when the context is disposed if
        /// contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">
        /// If set to true the connection is disposed when the context is disposed, otherwise the
        /// caller must dispose the connection.
        /// </param>
        public IdentityContextMysql(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes it from the
        /// given model. See the class remarks for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public IdentityContextMysql(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        internal static bool IsIdentityV1Schema(DbContext db)
        {
            var originalConnection = db.Database.Connection as MySqlConnection;

            // Give up and assume its ok if its not a sql connection
            if (originalConnection == null)
            {
                return false;
            }

            if (db.Database.Exists())
            {
                using (var tempConnection = new MySqlConnection(originalConnection.ConnectionString))
                {
                    tempConnection.Open();
                    return
                        VerifyColumns(tempConnection, "users", "Id", "UserName", "UserLogin", "PasswordHash", "SecurityStamp", "Discriminator") &&
                        VerifyColumns(tempConnection, "roles", "Id", "Name") &&
                        VerifyColumns(tempConnection, "userroles", "UserId", "RoleId") &&
                        VerifyColumns(tempConnection, "userclaims", "Id", "ClaimType", "ClaimValue", "UserId") &&
                        VerifyColumns(tempConnection, "userlogins", "UserId", "ProviderKey", "LoginProvider");
                }
            }

            return false;
        }

        internal static bool VerifyColumns(MySqlConnection conn, string table, params string[] columns)
        {
            var tableColumns = new List<string>();
            using (
                var command =
                    new MySqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME=@Table", conn))
            {
                command.Parameters.Add(new MySqlParameter("Table", table));
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Add all the columns from the table
                        tableColumns.Add(reader.GetString(0));
                    }
                }
            }

            // Make sure that we find all the expected columns
            return columns.All(tableColumns.Contains);
        }
    }

    /// <summary>
    /// Generic IdentityDbContext base that can be customized with entity types that extend from the
    /// base IdentityUserXXX types.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUserLogin"></typeparam>
    /// <typeparam name="TUserRole"></typeparam>
    /// <typeparam name="TUserClaim"></typeparam>
    public class IdentityContextMysql<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim> : DbContext
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
    {

        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString
        /// </summary>
        public IdentityContextMysql()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string to use
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public IdentityContextMysql(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model. The connection will not be disposed
        /// when the context is disposed if contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">
        /// Constructs a new context instance using the existing connection to connect to a
        /// database, and initializes it from the given model. The connection will not be disposed
        /// when the context is disposed if contextOwnsConnection is false.
        /// </param>
        public IdentityContextMysql(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using conventions to create the name of the database
        /// to which a connection will be made, and initializes it from the given model. The
        /// by-convention name is the full name (namespace + class
        /// name) of the derived context class. See the class remarks for how this is used to create
        ///       a connection.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        public IdentityContextMysql(DbCompiledModel model)
            : base(model)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the existing connection to connect to a
        /// database. The connection will not be disposed when the context is disposed if
        /// contextOwnsConnection is false.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">
        /// If set to true the connection is disposed when the context is disposed, otherwise the
        /// caller must dispose the connection.
        /// </param>
        public IdentityContextMysql(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Constructs a new context instance using the given string as the name or connection
        /// string for the database to which a connection will be made, and initializes it from the
        /// given model. See the class remarks for how this is used to create a connection.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        public IdentityContextMysql(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }

        /// <summary>
        /// If true validates that emails are unique
        /// </summary>
        public bool RequireUniqueEmail { get; set; }

        /// <summary>
        /// IDbSet of Roles
        /// </summary>
        public virtual IDbSet<TRole> Roles { get; set; }

        /// <summary>
        /// IDbSet of Users
        /// </summary>
        public virtual IDbSet<TUser> Users { get; set; }

        /// <summary>
        /// Maps table names, and sets up relationships between the various user entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            // Needed to ensure subclasses share the same table
            var user = modelBuilder.Entity<TUser>().ToTable("users");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserLogin_UNIQUE") { IsUnique = true }));

            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<TUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("userroles");

            modelBuilder.Entity<TUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("userlogins");

            modelBuilder.Entity<TUserClaim>()
                .ToTable("userclaims");

            var role = modelBuilder.Entity<TRole>()
                .ToTable("roles");
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Name_UNIQUE") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
        }

        /// <summary>
        /// Validates that UserNames are unique and case insenstive
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                var errors = new List<DbValidationError>();
                var user = entityEntry.Entity as TUser;

                //Check for uniqueness of user login
                if (user != null)
                {
                    if (Users.Any(u => String.Equals(u.UserLogin, user.UserLogin)))
                    {
                        errors.Add(new DbValidationError("User", String.Format(CultureInfo.CurrentCulture, IdentityResources.DuplicateUserLogin, user.UserLogin)));
                    }
                }
                else
                {
                    var role = entityEntry.Entity as TRole;

                    //check for uniqueness of role name
                    if (role != null && Roles.Any(r => String.Equals(r.Name, role.Name)))
                    {
                        errors.Add(new DbValidationError("Role", String.Format(CultureInfo.CurrentCulture, IdentityResources.RoleAlreadyExists, role.Name)));
                    }
                }
                if (errors.Any())
                {
                    return new DbEntityValidationResult(entityEntry, errors);
                }
            }
            return base.ValidateEntity(entityEntry, items);
        }
    }
}