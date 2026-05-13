import { Pressable, StyleSheet, Text, View } from 'react-native';
import { TypDnia } from '@/api/types';

const LABELS: Record<TypDnia, string> = {
  ROB: 'Roboczy',
  SOB: 'Sobota',
  SWI: 'Niedz./Święto',
};

type Props = {
  value: TypDnia;
  onChange: (v: TypDnia) => void;
};

export function DayTypePicker({ value, onChange }: Props) {
  return (
    <View style={styles.row}>
      {(Object.keys(LABELS) as TypDnia[]).map((k) => {
        const active = k === value;
        return (
          <Pressable
            key={k}
            onPress={() => onChange(k)}
            style={[styles.btn, active && styles.btnActive]}
          >
            <Text style={[styles.text, active && styles.textActive]}>{LABELS[k]}</Text>
          </Pressable>
        );
      })}
    </View>
  );
}

const styles = StyleSheet.create({
  row: { flexDirection: 'row', gap: 8, padding: 12 },
  btn: {
    flex: 1,
    paddingVertical: 10,
    borderRadius: 6,
    borderWidth: 1,
    borderColor: '#d1d5db',
    alignItems: 'center',
    backgroundColor: '#fff',
  },
  btnActive: { backgroundColor: '#1d4ed8', borderColor: '#1d4ed8' },
  text: { color: '#374151', fontWeight: '600' },
  textActive: { color: '#fff' },
});
