import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { useEffect, useState } from 'react';
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
import { PaginatedList, PrzystanekListDto } from '@/api/types';
import { ErrorView } from '@/components/ErrorView';
import { RootStackParamList } from '@/navigation/types';

type Props = NativeStackScreenProps<RootStackParamList, 'StopPicker'>;

export function StopPickerScreen({ navigation, route }: Props) {
  const { onSelect } = route.params;
  const [query, setQuery] = useState('');
  const [items, setItems] = useState<PrzystanekListDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    const handle = setTimeout(async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await apiGet<PaginatedList<PrzystanekListDto>>('/przystanki', {
          szukaj: query || undefined,
          pageSize: 50,
        });
        if (!cancelled) setItems(data.items);
      } catch (e) {
        if (!cancelled) setError((e as Error).message);
      } finally {
        if (!cancelled) setLoading(false);
      }
    }, 300);
    return () => {
      cancelled = true;
      clearTimeout(handle);
    };
  }, [query]);

  return (
    <View style={styles.container}>
      <TextInput
        placeholder="Szukaj przystanku…"
        value={query}
        onChangeText={setQuery}
        style={styles.input}
        autoFocus
      />
      {loading && <ActivityIndicator style={{ margin: 12 }} />}
      {error && <ErrorView message={error} onRetry={() => setQuery((q) => q + '')} />}
      {!loading && !error && items.length === 0 && (
        <Text style={styles.empty}>Brak wyników.</Text>
      )}
      <FlatList
        data={items}
        keyExtractor={(it) => String(it.id)}
        renderItem={({ item }) => (
          <Pressable
            style={styles.row}
            onPress={() => {
              onSelect(item);
              navigation.goBack();
            }}
          >
            <Text style={styles.rowTitle}>{item.nazwa}</Text>
            <Text style={styles.rowMeta}>
              {item.ulica ? `${item.ulica} · ` : ''}
              {item.dzielnica}
            </Text>
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
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e5e7eb',
    backgroundColor: '#fff',
  },
  rowTitle: { fontSize: 16, color: '#111827', fontWeight: '500' },
  rowMeta: { fontSize: 12, color: '#6b7280', marginTop: 2 },
});
