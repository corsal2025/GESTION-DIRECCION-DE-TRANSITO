using Microsoft.Extensions.Logging;
using ValpoControl.Core.Domain.Entities;
using ValpoControl.Infrastructure.Persistence;

namespace ValpoControl.Infrastructure.Services;

// Servicios específicos para cada módulo - heredan de BaseTramiteService<T>

public class GestionF8Service : BaseTramiteService<GestionF8>
{
    public GestionF8Service(ValpoControlDbContext context, ILogger<GestionF8Service> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class Modulo12Service : BaseTramiteService<Modulo12>
{
    public Modulo12Service(ValpoControlDbContext context, ILogger<Modulo12Service> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class Modulo16Service : BaseTramiteService<Modulo16>
{
    public Modulo16Service(ValpoControlDbContext context, ILogger<Modulo16Service> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class HojaRutaService : BaseTramiteService<HojaRuta>
{
    public HojaRutaService(ValpoControlDbContext context, ILogger<HojaRutaService> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class ExamenPsicometricoService : BaseTramiteService<ExamenPsicometrico>
{
    public ExamenPsicometricoService(ValpoControlDbContext context, ILogger<ExamenPsicometricoService> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class ExamenTeoricoService : BaseTramiteService<ExamenTeorico>
{
    public ExamenTeoricoService(ValpoControlDbContext context, ILogger<ExamenTeoricoService> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class CambioDomicilioService : BaseTramiteService<CambioDomicilio>
{
    public CambioDomicilioService(ValpoControlDbContext context, ILogger<CambioDomicilioService> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class GestionLicenciasService : BaseTramiteService<GestionLicencias>
{
    public GestionLicenciasService(ValpoControlDbContext context, ILogger<GestionLicenciasService> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}

public class ArchivoPlanoService : BaseTramiteService<ArchivoPlano>
{
    public ArchivoPlanoService(ValpoControlDbContext context, ILogger<ArchivoPlanoService> logger, string rutaBaseServidor)
        : base(context, logger, rutaBaseServidor) { }
}
