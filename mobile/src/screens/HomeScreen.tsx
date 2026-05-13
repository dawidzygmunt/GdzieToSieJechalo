import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { Pressable, StyleSheet, Text, View } from 'react-native';
import { RootStackParamList } from '@/navigation/types';

type Props = NativeStackScreenProps<RootStackParamList, 'Home'>;

export function HomeScreen({ navigation }: Props) {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>GdzieToSieJechało</Text>
      <Text style={styles.subtitle}>Komunikacja miejska — wyszukiwarka i rozkłady</Text>

      <Pressable
        style={[styles.tile, styles.tilePrimary]}
        onPress={() => navigation.navigate('Polaczenia')}
      >
        <Text style={styles.tileTitle}>Wyszukaj połączenie</Text>
        <Text style={styles.tileText}>Z punktu A do punktu B</Text>
      </Pressable>

      <Pressable
        style={[styles.tile, styles.tileSecondary]}
        onPress={() => navigation.navigate('Linie')}
      >
        <Text style={styles.tileTitle}>Rozkład linii</Text>
        <Text style={styles.tileText}>Pełen rozkład wybranej linii</Text>
      </Pressable>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, padding: 24, gap: 16, backgroundColor: '#f9fafb' },
  title: { fontSize: 28, fontWeight: '700', color: '#111827', marginTop: 24 },
  subtitle: { fontSize: 14, color: '#6b7280', marginBottom: 16 },
  tile: {
    padding: 20,
    borderRadius: 10,
    gap: 6,
  },
  tilePrimary: { backgroundColor: '#1d4ed8' },
  tileSecondary: { backgroundColor: '#0f766e' },
  tileTitle: { color: '#fff', fontSize: 20, fontWeight: '700' },
  tileText: { color: '#e5e7eb', fontSize: 14 },
});
