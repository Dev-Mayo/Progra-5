import 'package:flutter/material.dart';

import '../widgets/app_confirmation_dialog.dart';
import '../widgets/app_feedback.dart';

class FeaturePageTemplate extends StatelessWidget {
  const FeaturePageTemplate({
    super.key,
    required this.title,
    required this.subtitle,
    required this.icon,
    required this.accentColor,
    required this.actionLabel,
    required this.onPrimaryAction,
  });

  final String title;
  final String subtitle;
  final IconData icon;
  final Color accentColor;
  final String actionLabel;
  final Future<void> Function(BuildContext context) onPrimaryAction;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);

    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        elevation: 0,
        title: Text(title),
      ),
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Container(
                width: double.infinity,
                padding: const EdgeInsets.all(20),
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [accentColor, accentColor.withValues(alpha: 0.78)],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.circular(24),
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Icon(icon, color: Colors.white, size: 36),
                    const SizedBox(height: 16),
                    Text(
                      title,
                      style: theme.textTheme.headlineSmall?.copyWith(
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 8),
                    Text(
                      subtitle,
                      style: theme.textTheme.bodyMedium?.copyWith(
                        color: Colors.white.withValues(alpha: 0.92),
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 20),
              Text(
                'Area de contenido general',
                style: theme.textTheme.titleMedium?.copyWith(
                  fontWeight: FontWeight.w700,
                ),
              ),
              const SizedBox(height: 12),
              Expanded(
                child: ListView(
                  children: [
                    _InfoCard(
                      title: 'Pantalla activa',
                      description: title,
                      icon: icon,
                    ),
                    const SizedBox(height: 12),
                    const _InfoCard(
                      title: 'Espacio para el equipo',
                      description:
                          'Espacio en el que se cambia informacion o funcionalidad.',
                      icon: Icons.groups_rounded,
                    ),
                    const SizedBox(height: 12),
                    const _InfoCard(
                      title: 'Componentes compartidos',
                      description:
                          'Puedes reutilizar snackbar, toast y dialogo de confirmacion en este modulo.',
                      icon: Icons.widgets_rounded,
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 12),
              SizedBox(
                width: double.infinity,
                child: FilledButton.icon(
                  onPressed: () => onPrimaryAction(context),
                  icon: const Icon(Icons.play_arrow_rounded),
                  label: Text(actionLabel),
                ),
              ),
            ],
          ),
        ),
      ),
      bottomNavigationBar: BottomAppBar(
        color: Colors.white,
        elevation: 8,
        child: SafeArea(
          top: false,
          child: SizedBox(
            height: 64,
            child: InkWell(
              onTap: () => Navigator.of(context).pop(),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(Icons.home_rounded, color: theme.colorScheme.primary),
                  Text(
                    'Inicio',
                    style: theme.textTheme.labelMedium?.copyWith(
                      color: theme.colorScheme.primary,
                      fontWeight: FontWeight.w700,
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class _InfoCard extends StatelessWidget {
  const _InfoCard({
    required this.title,
    required this.description,
    required this.icon,
  });

  final String title;
  final String description;
  final IconData icon;

  @override
  Widget build(BuildContext context) {
    final colorScheme = Theme.of(context).colorScheme;

    return Card(
      elevation: 0,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Row(
          children: [
            Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: colorScheme.primaryContainer,
                borderRadius: BorderRadius.circular(16),
              ),
              child: Icon(icon, color: colorScheme.primary),
            ),
            const SizedBox(width: 14),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    title,
                    style: Theme.of(context).textTheme.titleSmall?.copyWith(
                      fontWeight: FontWeight.w700,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(description),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class InscribirPage extends StatelessWidget {
  const InscribirPage({super.key});

  @override
  Widget build(BuildContext context) {
    return FeaturePageTemplate(
      title: 'Inscribir cuenta',
      subtitle: 'Registra cuentas frecuentes para pagos y transferencias.',
      icon: Icons.person_add_alt_1_rounded,
      accentColor: const Color(0xFF00695C),
      actionLabel: 'Mostrar toast',
      onPrimaryAction: (context) async {
        AppFeedback.showToast(
          context,
          message: 'Cuenta inscrita correctamente.',
        );
      },
    );
  }
}

class DesinscribirPage extends StatelessWidget {
  const DesinscribirPage({super.key});

  @override
  Widget build(BuildContext context) {
    return FeaturePageTemplate(
      title: 'Desinscribir cuenta',
      subtitle: 'Gestiona la salida de cuentas previamente afiliadas.',
      icon: Icons.person_remove_alt_1_rounded,
      accentColor: const Color(0xFFB05E27),
      actionLabel: 'Mostrar snackbar',
      onPrimaryAction: (context) async {
        AppFeedback.showSnackbar(
          context,
          message: 'Solicitud de desinscripcion enviada.',
          actionLabel: 'Cerrar',
        );
      },
    );
  }
}

class VerSaldoPage extends StatelessWidget {
  const VerSaldoPage({super.key});

  @override
  Widget build(BuildContext context) {
    return FeaturePageTemplate(
      title: 'Ver saldo',
      subtitle: 'Consulta el balance disponible de manera rapida.',
      icon: Icons.account_balance_wallet_rounded,
      accentColor: const Color(0xFF1565C0),
      actionLabel: 'Actualizar saldo',
      onPrimaryAction: (context) async {
        AppFeedback.showSnackbar(
          context,
          message: 'Saldo disponible: CRC 245,300.00',
        );
      },
    );
  }
}

class VerMovimientosPage extends StatelessWidget {
  const VerMovimientosPage({super.key});

  @override
  Widget build(BuildContext context) {
    return FeaturePageTemplate(
      title: 'Ver movimientos',
      subtitle: 'Revisa los movimientos recientes de la cuenta principal.',
      icon: Icons.receipt_long_rounded,
      accentColor: const Color(0xFF6A1B9A),
      actionLabel: 'Cargar movimientos',
      onPrimaryAction: (context) async {
        AppFeedback.showSnackbar(
          context,
          message: 'Movimientos actualizados.',
          actionLabel: 'Ver',
        );
      },
    );
  }
}

class TransferenciaPage extends StatelessWidget {
  const TransferenciaPage({super.key});

  @override
  Widget build(BuildContext context) {
    return FeaturePageTemplate(
      title: 'Realizar transferencia',
      subtitle: 'Ejecuta transferencias seguras con confirmacion previa.',
      icon: Icons.swap_horiz_rounded,
      accentColor: const Color(0xFFC62828),
      actionLabel: 'Confirmar transferencia',
      onPrimaryAction: (context) async {
        final confirmed = await AppConfirmationDialog.show(
          context,
          title: 'Confirmar transferencia',
          message: 'Esta accion simula una transferencia. ¿Deseas continuar?',
          confirmLabel: 'Transferir',
        );

        if (!context.mounted) {
          return;
        }

        if (confirmed) {
          AppFeedback.showSnackbar(
            context,
            message: 'Transferencia realizada con exito.',
          );
        } else {
          AppFeedback.showToast(context, message: 'Transferencia cancelada.');
        }
      },
    );
  }
}
