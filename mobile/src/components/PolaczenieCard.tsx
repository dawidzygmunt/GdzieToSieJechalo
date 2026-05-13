import { StyleSheet, Text, View } from 'react-native';
import { PolaczenieDto } from '@/api/types';

function trimSeconds(t: string): string {
  return t.length >= 5 ? t.slice(0, 5) : t;
}

export function PolaczenieCard({ p }: { p: PolaczenieDto }) {
  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <View style={styles.lineBadge}>
          <Text style={styles.lineText}>{p.numerLinii}</Text>
        </View>
        <Text style={styles.kierunek} numberOfLines={1}>
          → {p.kierunek}
        </Text>
      </View>

      <View style={styles.times}>
        <Text style={styles.time}>{trimSeconds(p.godzinaOdjazdu)}</Text>
        <Text style={styles.arrow}>→</Text>
        <Text style={styles.time}>{trimSeconds(p.godzinaPrzyjazdu)}</Text>
        <Text style={styles.duration}>{p.czasTrwaniaMin} min</Text>
      </View>

      <Text style={styles.stops} numberOfLines={1}>
        {p.nazwaPrzystankuZ} → {p.nazwaPrzystankuDo}
      </Text>
      <Text style={styles.meta}>{p.liczbaPrzystankowPosrednich} przystanków pośrednich</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    backgroundColor: '#fff',
    borderRadius: 8,
    padding: 14,
    marginVertical: 6,
    borderWidth: 1,
    borderColor: '#e5e7eb',
  },
  header: { flexDirection: 'row', alignItems: 'center', gap: 10, marginBottom: 8 },
  lineBadge: {
    backgroundColor: '#1d4ed8',
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 4,
  },
  lineText: { color: '#fff', fontWeight: '700' },
  kierunek: { flex: 1, fontSize: 14, color: '#374151' },
  times: { flexDirection: 'row', alignItems: 'center', gap: 8, marginBottom: 6 },
  time: { fontSize: 18, fontWeight: '700', color: '#111827' },
  arrow: { fontSize: 16, color: '#6b7280' },
  duration: { marginLeft: 'auto', fontSize: 13, color: '#6b7280' },
  stops: { fontSize: 13, color: '#374151' },
  meta: { fontSize: 12, color: '#6b7280', marginTop: 2 },
});
