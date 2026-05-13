import { Pressable, StyleSheet, Text, View } from 'react-native';

type Props = {
  message: string;
  onRetry?: () => void;
};

export function ErrorView({ message, onRetry }: Props) {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Coś poszło nie tak</Text>
      <Text style={styles.message}>{message}</Text>
      {onRetry && (
        <Pressable style={styles.button} onPress={onRetry}>
          <Text style={styles.buttonText}>Spróbuj ponownie</Text>
        </Pressable>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: { padding: 24, alignItems: 'center', gap: 12 },
  title: { fontSize: 18, fontWeight: '600', color: '#b91c1c' },
  message: { fontSize: 14, color: '#444', textAlign: 'center' },
  button: {
    marginTop: 8,
    backgroundColor: '#1f2937',
    paddingHorizontal: 16,
    paddingVertical: 10,
    borderRadius: 6,
  },
  buttonText: { color: '#fff', fontWeight: '600' },
});
