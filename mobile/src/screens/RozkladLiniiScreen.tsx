import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { useEffect, useState } from 'react';
import {
  ActivityIndicator,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from 'react-native';
import { apiGet } from '@/api/client';
import { KursDto, RozkladLiniiDto, TypDnia, WariantRozkladDto } from '@/api/types';
import { DayTypePicker } from '@/components/DayTypePicker';
import { ErrorView } from '@/components/ErrorView';
import { RootStackParamList } from '@/navigation/types';

type Props = NativeStackScreenProps<RootStackParamList, 'RozkladLinii'>;

function trimSeconds(t: string): string {
  return t.length >= 5 ? t.slice(0, 5) : t;
}

function getStops(kursy: KursDto[]): string[] {
  const longest = kursy.reduce<KursDto | null>(
    (acc, k) => (acc && acc.przystanki.length >= k.przystanki.length ? acc : k),
    null
  );
  return longest?.przystanki.map((p) => p.nazwaPrzystanku) ?? [];
}

function findTime(kurs: KursDto, stopName: string): string | null {
  const s = kurs.przystanki.find((p) => p.nazwaPrzystanku === stopName);
  return s ? trimSeconds(s.godzinaOdjazdu) : null;
}

function WariantSection({ wariant }: { wariant: WariantRozkladDto }) {
  const stops = getStops(wariant.kursy);

  return (
    <View style={styles.wariant}>
      <Text style={styles.wariantTitle}>{wariant.nazwaWariantu}</Text>
      <Text style={styles.wariantDir}>→ {wariant.kierunek}</Text>

      {wariant.kursy.length === 0 ? (
        <Text style={styles.empty}>Brak kursów w tym wariancie.</Text>
      ) : (
        <ScrollView horizontal showsHorizontalScrollIndicator>
          <View>
            <View style={styles.headerRow}>
              <View style={[styles.cell, styles.stopCell, styles.header]}>
                <Text style={styles.headerText}>Przystanek</Text>
              </View>
              {wariant.kursy.map((k) => (
                <View key={k.nrKursu} style={[styles.cell, styles.timeCell, styles.header]}>
                  <Text style={styles.headerText}>#{k.nrKursu}</Text>
                </View>
              ))}
            </View>

            {stops.map((stop) => (
              <View key={stop} style={styles.dataRow}>
                <View style={[styles.cell, styles.stopCell]}>
                  <Text style={styles.stopText} numberOfLines={1}>
                    {stop}
                  </Text>
                </View>
                {wariant.kursy.map((k) => {
                  const time = findTime(k, stop);
                  return (
                    <View key={k.nrKursu} style={[styles.cell, styles.timeCell]}>
                      <Text style={styles.timeText}>{time ?? '—'}</Text>
                    </View>
                  );
                })}
              </View>
            ))}
          </View>
        </ScrollView>
      )}
    </View>
  );
}

export function RozkladLiniiScreen({ route, navigation }: Props) {
  const { id, numerLinii } = route.params;
  const [typDnia, setTypDnia] = useState<TypDnia>('ROB');
  const [data, setData] = useState<RozkladLiniiDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    navigation.setOptions({ title: `Linia ${numerLinii}` });
  }, [navigation, numerLinii]);

  const load = async (t: TypDnia) => {
    setLoading(true);
    setError(null);
    setData(null);
    try {
      const r = await apiGet<RozkladLiniiDto>(`/linie/${id}/rozklad`, { typDnia: t });
      setData(r);
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load(typDnia);
  }, [typDnia, id]);

  return (
    <View style={styles.container}>
      <DayTypePicker value={typDnia} onChange={setTypDnia} />

      {loading && <ActivityIndicator style={{ margin: 16 }} />}
      {error && <ErrorView message={error} onRetry={() => load(typDnia)} />}

      {data && (
        <ScrollView contentContainerStyle={{ paddingBottom: 24 }}>
          {data.warianty.length === 0 ? (
            <Text style={styles.empty}>Brak wariantów dla tego typu dnia.</Text>
          ) : (
            data.warianty.map((w, idx) => <WariantSection key={idx} wariant={w} />)
          )}
        </ScrollView>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f9fafb' },
  wariant: {
    backgroundColor: '#fff',
    margin: 12,
    padding: 12,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#e5e7eb',
  },
  wariantTitle: { fontSize: 16, fontWeight: '700', color: '#111827' },
  wariantDir: { fontSize: 13, color: '#6b7280', marginBottom: 10 },
  empty: { textAlign: 'center', color: '#6b7280', padding: 16 },
  headerRow: { flexDirection: 'row' },
  dataRow: { flexDirection: 'row' },
  cell: {
    paddingHorizontal: 8,
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#e5e7eb',
    justifyContent: 'center',
  },
  header: { backgroundColor: '#f3f4f6', borderBottomColor: '#d1d5db' },
  headerText: { fontWeight: '700', color: '#374151', fontSize: 12 },
  stopCell: { width: 160 },
  stopText: { fontSize: 13, color: '#111827' },
  timeCell: { width: 64, alignItems: 'center' },
  timeText: { fontSize: 13, color: '#111827', fontVariant: ['tabular-nums'] },
});
