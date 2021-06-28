rules = {
'...##' : '#',
'..#..' : '#',
'.#...' : '#',
'.#.#.' : '#',
'.#.##' : '#',
'.##..' : '#',
'.####' : '#',
'#.#.#' : '#',
'#.###' : '#',
'##.#.' : '#',
'##.##' : '#',
'###..' : '#',
'###.#' : '#',
'####.' : '#',
}

def step(s):
	res = []
	for i in range(len(s)):
		w = s[i-2:i+3]
		#print(w)
		res.append(rules.get(w, '.'))
	return ''.join(res)

def main():
	state = '#..#.#..##......###...###'
	for i in range(20):
		state = step(state)
		print(state)

if __name__ == '__main__':
	main()