import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { useState } from 'react';
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
import { PolaczenieDto, PrzystanekListDto } from '@/api/types';
import { ErrorView } from '@/components/ErrorView';
import { PolaczenieCard } from '@/components/PolaczenieCard';
import { RootStackParamList } from '@/navigation/types';

type Props = NativeStackScreenProps<RootStackParamList, 'Polaczenia'>;

function todayISO(): string {
  const d = new Date();
  const pad = (n: number) => String(n).padStart(2, '0');
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;
}

function nowHHmm(): string {
  const d = new Date();
  const pad = (n: number) => String(n).padStart(2, '0');
  return `${pad(d.getHours())}:${pad(d.getMinutes())}`;
}

const DATE_RE = /^\d{4}-\d{2}-\d{2}$/;
const TIME_RE = /^\d{2}:\d{2}(:\d{2})?$/;

export function PolaczeniaScreen({ navigation }: Props) {
  const [stopZ, setStopZ] = useState<PrzystanekListDto | null>(null);
  const [stopDo, setStopDo] = useState<PrzystanekListDto | null>(null);
  const [data, setData] = useState(todayISO());
  const [czas, setCzas] = useState(nowHHmm());
  const [results, setResults] = useState<PolaczenieDto[] | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const openPicker = (target: 'z' | 'do') => {
    navigation.navigate('StopPicker', {
      title: target === 'z' ? 'Skąd' : 'Dokąd',
      onSelect: (s) => (target === 'z' ? setStopZ(s) : setStopDo(s)),
    });
  };

  const onSearch = async () => {
    setError(null);
    if (!stopZ || !stopDo) {
      setError('Wybierz oba przystanki.');
      return;
    }
    if (stopZ.id === stopDo.id) {
      setError('Przystanek początkowy i końcowy muszą być różne.');
      return;
    }
    if (!DATE_RE.test(data)) {
      setError('Data musi mieć format YYYY-MM-DD.');
      return;
    }
    if (!TIME_RE.test(czas)) {
      setError('Czas musi mieć format HH:mm.');
      return;
    }

    setLoading(true);
    try {
      const czasNormalized = czas.length === 5 ? `${czas}:00` : czas;
      const list = await apiGet<PolaczenieDto[]>('/polaczenia', {
        przystanekZ: stopZ.id,
        przystanekDo: stopDo.id,
        data,
        czas: czasNormalized,
        maxWynikow: 10,
      });
      setResults(list);
    } catch (e) {
      setError((e as Error).message);
      setResults(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.form}>
        <Pressable style={styles.field} onPress={() => openPicker('z')}>
          <Text style={styles.label}>Skąd</Text>
          <Text style={[styles.value, !stopZ && styles.placeholder]}>
            {stopZ ? stopZ.nazwa : 'Wybierz przystanek…'}
          </Text>
        </Pressable>

        <Pressable style={styles.field} onPress={() => openPicker('do')}>
          <Text style={styles.label}>Dokąd</Text>
          <Text style={[styles.value, !stopDo && styles.placeholder]}>
            {stopDo ? stopDo.nazwa : 'Wybierz przystanek…'}
          </Text>
        </Pressable>

        <View style={styles.row}>
          <View style={[styles.field, styles.flex]}>
            <Text style={styles.label}>Data</Text>
            <TextInput
              value={data}
              onChangeText={setData}
              placeholder="YYYY-MM-DD"
              style={styles.input}
              autoCapitalize="none"
            />
          </View>
          <View style={[styles.field, styles.flex]}>
            <Text style={styles.label}>Czas</Text>
            <TextInput
              value={czas}
              onChangeText={setCzas}
              placeholder="HH:mm"
              style={styles.input}
              autoCapitalize="none"
            />
          </View>
        </View>

        <Pressable
          style={[styles.button, loading && styles.buttonDisabled]}
          onPress={onSearch}
          disabled={loading}
        >
          {loading ? (
            <ActivityIndicator color="#fff" />
          ) : (
            <Text style={styles.buttonText}>Szukaj połączeń</Text>
          )}
        </Pressable>
      </View>

      {error && <ErrorView message={error} />}
      {results && results.length === 0 && !error && (
        <Text style={styles.empty}>Brak połączeń dla wybranych kryteriów.</Text>
      )}

      <FlatList
        data={results ?? []}
        keyExtractor={(_, idx) => String(idx)}
        renderItem={({ item }) => <PolaczenieCard p={item} />}
        contentContainerStyle={styles.list}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f9fafb' },
  form: { padding: 12, gap: 10 },
  row: { flexDirection: 'row', gap: 10 },
  flex: { flex: 1 },
  field: {
    backgroundColor: '#fff',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    borderWidth: 1,
    borderColor: '#d1d5db',
  },
  label: { fontSize: 11, color: '#6b7280', textTransform: 'uppercase' },
  value: { fontSize: 16, color: '#111827', marginTop: 2 },
  placeholder: { color: '#9ca3af' },
  input: { fontSize: 16, color: '#111827', padding: 0, marginTop: 2 },
  button: {
    backgroundColor: '#1d4ed8',
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 4,
  },
  buttonDisabled: { backgroundColor: '#93c5fd' },
  buttonText: { color: '#fff', fontWeight: '700', fontSize: 16 },
  empty: { textAlign: 'center', padding: 16, color: '#6b7280' },
  list: { paddingHorizontal: 12, paddingBottom: 24 },
});
