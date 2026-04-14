import 'package:flutter/material.dart';

import 'feature_page_template.dart';

enum HomeMenuOption {
  inscribir,
  desinscribir,
  verSaldo,
  verMovimientos,
  transferencia,
}

class HomeTemplatePage extends StatefulWidget {
  const HomeTemplatePage({super.key});

  @override
  State<HomeTemplatePage> createState() => _HomeTemplatePageState();
}

class _HomeTemplatePageState extends State<HomeTemplatePage> {
  int _currentTabIndex = 0;

  Future<void> _handleMenuSelection(HomeMenuOption option) async {
    await Navigator.of(
      context,
    ).push(MaterialPageRoute(builder: (_) => _pageFor(option)));
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    final isHomeSelected = _currentTabIndex == 0;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.white,
        elevation: 0,
        titleSpacing: 16,
        title: Row(
          children: [
            Container(
              width: 40,
              height: 40,
              decoration: BoxDecoration(
                color: theme.colorScheme.primary,
                borderRadius: BorderRadius.circular(12),
              ),
              child: const Icon(Icons.account_balance, color: Colors.white),
            ),
            const SizedBox(width: 12),
            const Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisSize: MainAxisSize.min,
                children: [
                  Text(
                    'Sinpefy',
                    style: TextStyle(fontWeight: FontWeight.w700),
                  ),
                  Text('Sistema Pagos Moviles', style: TextStyle(fontSize: 12)),
                ],
              ),
            ),
          ],
        ),
        actions: [
          PopupMenuButton<HomeMenuOption>(
            tooltip: 'Opciones',
            onSelected: _handleMenuSelection,
            itemBuilder: (context) => HomeMenuOption.values
                .map(
                  (option) => PopupMenuItem<HomeMenuOption>(
                    value: option,
                    child: Text(_labelFor(option)),
                  ),
                )
                .toList(),
            icon: const Icon(Icons.menu_rounded),
          ),
          const SizedBox(width: 8),
        ],
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
                    colors: [
                      theme.colorScheme.primary,
                      theme.colorScheme.primary.withValues(alpha: 0.78),
                    ],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ),
                  borderRadius: BorderRadius.circular(24),
                ),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Icon(
                      Icons.dashboard_customize_rounded,
                      color: Colors.white,
                      size: 36,
                    ),
                    const SizedBox(height: 16),
                    Text(
                      'Plantilla principal',
                      style: theme.textTheme.headlineSmall?.copyWith(
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    const SizedBox(height: 8),
                    Text(
                      'Usa el menu superior para entrar a cada modulo en su propia pantalla independiente.',
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
              const Expanded(child: _HomeOverview()),
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
              onTap: () {
                setState(() {
                  _currentTabIndex = 0;
                });
              },
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Icon(
                    Icons.home_rounded,
                    color: isHomeSelected
                        ? theme.colorScheme.primary
                        : theme.colorScheme.onSurfaceVariant,
                  ),
                  Text(
                    'Inicio',
                    style: theme.textTheme.labelMedium?.copyWith(
                      color: isHomeSelected
                          ? theme.colorScheme.primary
                          : theme.colorScheme.onSurfaceVariant,
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

  static String _labelFor(HomeMenuOption option) {
    switch (option) {
      case HomeMenuOption.inscribir:
        return 'Inscribir';
      case HomeMenuOption.desinscribir:
        return 'Desinscribir';
      case HomeMenuOption.verSaldo:
        return 'Ver Saldo';
      case HomeMenuOption.verMovimientos:
        return 'Ver Movimientos';
      case HomeMenuOption.transferencia:
        return 'Realizar Transferencia';
    }
  }

  Widget _pageFor(HomeMenuOption option) {
    switch (option) {
      case HomeMenuOption.inscribir:
        return const InscribirPage();
      case HomeMenuOption.desinscribir:
        return const DesinscribirPage();
      case HomeMenuOption.verSaldo:
        return const VerSaldoPage();
      case HomeMenuOption.verMovimientos:
        return const VerMovimientosPage();
      case HomeMenuOption.transferencia:
        return const TransferenciaPage();
    }
  }
}

class _HomeOverview extends StatelessWidget {
  const _HomeOverview();

  @override
  Widget build(BuildContext context) {
    return ListView(
      children: const [
        _InfoCard(
          title: 'Navegacion por modulos',
          description:
              'Cada opcion del menu abre una pantalla separada para aislar el trabajo del equipo.',
          icon: Icons.alt_route_rounded,
        ),
        SizedBox(height: 12),
        _InfoCard(
          title: 'Navegacion inferior',
          description: 'La plantilla incluye bottom tab con la seccion Inicio.',
          icon: Icons.home_rounded,
        ),
        SizedBox(height: 12),
        _InfoCard(
          title: 'Componentes reutilizables',
          description:
              'Snackbar, toast y dialogo de confirmacion siguen disponibles para todos los modulos.',
          icon: Icons.widgets_rounded,
        ),
      ],
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
