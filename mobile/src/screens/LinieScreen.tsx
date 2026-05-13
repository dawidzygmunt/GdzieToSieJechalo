import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { useEffect, useMemo, useState } from 'react';
import {
  ActivityIndicator,
  FlatList,
  Pressable,
  StyleSheet,
  Text,
  TextInput,
  View,
} from 'react-native';
import { apiGet } from '@/api/client';
import { LiniaListDto, PaginatedList } from '@/api/types';
import { ErrorView } from '@/components/ErrorView';
import { RootStackParamList } from '@/navigation/types';

type Props = NativeStackScreenProps<RootStackParamList, 'Linie'>;

export function LinieScreen({ navigation }: Props) {
  const [all, setAll] = useState<LiniaListDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [query, setQuery] = useState('');

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await apiGet<PaginatedList<LiniaListDto>>('/linie', { pageSize: 100 });
      setAll(data.items);
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const filtered = useMemo(() => {
    if (!query.trim()) return all;
    const q = query.trim().toLowerCase();
    return all.filter(
      (l) =>
        l.numerLinii.toLowerCase().includes(q) ||
        l.typLinii.toLowerCase().includes(q)
    );
  }, [all, query]);

  return (
    <View style={styles.container}>
      <TextInput
        placeholder="Szukaj linii…"
        value={query}
        onChangeText={setQuery}
        style={styles.input}
      />
      {loading && <ActivityIndicator style={{ margin: 16 }} />}
      {error && <ErrorView message={error} onRetry={load} />}
      {!loading && !error && filtered.length === 0 && (
        <Text style={styles.empty}>Brak linii.</Text>
      )}
      <FlatList
        data={filtered}
        keyExtractor={(it) => String(it.id)}
        renderItem={({ item }) => (
          <Pressable
            style={styles.row}
            onPress={() =>
              navigation.navigate('RozkladLinii', {
                id: item.id,
                numerLinii: item.numerLinii,
              })
            }
          >
            <View style={styles.badge}>
              <Text style={styles.badgeText}>{item.numerLinii}</Text>
            </View>
            <View style={styles.rowText}>
              <Text style={styles.rowTitle}>{item.typLinii}</Text>
              {item.opis && (
                <Text style={styles.rowMeta} numberOfLines={1}>
                  {item.opis}
                </Text>
              )}
              <Text style={styles.rowMeta}>
                Warianty: {item.liczbaWariantow} · {item.aktywna ? 'aktywna' : 'nieaktywna'}
              </Text>
            </View>
          </Pressable>
        )}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f9fafb' },
  input: {
    margin: 12,
    paddingHorizontal: 14,
    paddingVertical: 10,
    fontSize: 16,
    borderWidth: 1,
    borderColor: '#d1d5db',
    borderRadius: 8,
    backgroundColor: '#fff',
  },
  empty: { textAlign: 'center', padding: 16, color: '#6b7280' },
  row: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e5e7eb',
    backgroundColor: '#fff',
  },
  badge: {
    minWidth: 56,
    paddingHorizontal: 10,
    paddingVertical: 6,
    backgroundColor: '#0f766e',
    borderRadius: 4,
    alignItems: 'center',
  },
  badgeText: { color: '#fff', fontWeight: '700', fontSize: 16 },
  rowText: { flex: 1 },
  rowTitle: { fontSize: 14, color: '#111827', fontWeight: '500' },
  rowMeta: { fontSize: 12, color: '#6b7280' },
});
