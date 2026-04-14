import 'package:flutter_test/flutter_test.dart';
import 'package:flutter/material.dart';

import 'package:flutter_application_1/app.dart';

void main() {
  testWidgets('renders pagos moviles home template', (
    WidgetTester tester,
  ) async {
    await tester.pumpWidget(const PagosMovilesApp());

    expect(find.text('Sinpefy'), findsOneWidget);
    expect(find.text('Inicio'), findsOneWidget);
    expect(find.text('Plantilla principal'), findsOneWidget);
  });

  testWidgets('opens inscribir page from menu', (WidgetTester tester) async {
    await tester.pumpWidget(const PagosMovilesApp());

    await tester.tap(find.byIcon(Icons.menu_rounded));
    await tester.pumpAndSettle();
    await tester.tap(find.text('Inscribir'));
    await tester.pumpAndSettle();

    expect(find.text('Inscribir cuenta'), findsAtLeastNWidgets(1));
  });
}
