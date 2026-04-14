import 'package:flutter/material.dart';

import 'pages/home_template_page.dart';

class PagosMovilesApp extends StatelessWidget {
  const PagosMovilesApp({super.key});

  @override
  Widget build(BuildContext context) {
    //const seedColor = Color(0xFF00695C);

    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Pagos Móviles',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color.fromARGB(255, 3, 76, 125),
        ),
        scaffoldBackgroundColor: const Color(0xFFF4F8F7),
        snackBarTheme: const SnackBarThemeData(
          behavior: SnackBarBehavior.floating,
        ),
        useMaterial3: true,
      ),
      home: const HomeTemplatePage(),
    );
  }
}
