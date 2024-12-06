import { Pen, Plus, X } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Form, FormDrawer, Input, Select } from '@/components/ui/form'
import { useNotifications } from '@/components/ui/notifications'

import { useDirector } from '../api/get-director'
import { updateDirectorInputSchema, useUpdateDirector } from '../api/update-director'
import { useAllMovies } from '@/features/movies/api/get-movies'
import { Movie } from '@/types/api'
import { useState } from 'react'

type UpdateDirectorProps = {
  uniqueId: string
}

export const UpdateDirector = ({ uniqueId }: UpdateDirectorProps) => {
  const { addNotification } = useNotifications()
  const directorsQuery = useDirector({ uniqueId })
  const moviesAllQuery = useAllMovies({})

  const mutation = useUpdateDirector({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Director Updated',
          message: 'The director was successfull'
        })

        directorsQuery.refetch()
      }
    }
  })

  const director = directorsQuery.data?.data
  const [reactiveMovies, setReactiveMovies] = useState(director?.movies || [])
  const [selectedMovie, setSelectedMovie] = useState<string | null>(null)

  const addMovie = (uniqueId: string) => {
    if (uniqueId) {
      const movie = moviesAllQuery.data?.find((movie: Movie) => movie.uniqueId === uniqueId)
      const movieExists = reactiveMovies.find((movie) => movie.uniqueId === uniqueId)
      if (movie && !movieExists) setReactiveMovies((prevMovies) => [...prevMovies, movie])
    }
    setSelectedMovie(null)
  }

  const removeMovie = (uniqueId: string) => {
    setReactiveMovies((prevMovies) => prevMovies.filter((movie) => movie.uniqueId !== uniqueId))
  }

  return (
    <FormDrawer
      isDone={mutation.isSuccess}
      triggerButton={
        <Button type="submit" size="sm" icon={<Pen className="size-4" />}>
          Update Director
        </Button>
      }
      title="Update Director"
      submitButton={
        <>
          <Button form="update-director" type="submit" size="sm" isLoading={mutation.isPending}>
            Submit
          </Button>
        </>
      }
    >
      <Form
        id="update-director"
        onSubmit={(values) => {
          values.movies = reactiveMovies
          mutation.mutate({
            data: values,
            uniqueId
          })
        }}
        options={{
          defaultValues: {
            uniqueId: director?.uniqueId ?? '',
            movies: director?.movies ?? [],
            name: director?.name ?? ''
          }
        }}
        schema={updateDirectorInputSchema}
      >
        {({ register, formState }) => {
          return (
            <>
              <Input
                label="Name"
                error={formState.errors['name']}
                registration={register('name')}
              />

              <div className="grid grid-cols-2 gap-4 content-end">
                <Select
                  label="Movies"
                  className="border border-gray-300 rounded-md"
                  defaultValue={null}
                  options={(moviesAllQuery?.data ?? []).map((movie: Movie) => ({
                    value: movie.uniqueId,
                    label: movie.title
                  }))}
                  registration={register('movies', {
                    onChange: (e) => setSelectedMovie(e.target.value)
                  })}
                ></Select>
                <div className="content-end">
                  <Button
                    type="button"
                    icon={<Plus className="size-4" />}
                    onClick={() => selectedMovie && addMovie(selectedMovie)}
                  >
                    Add
                  </Button>
                </div>
              </div>

              <ul>
                {reactiveMovies.map((movie, index) => (
                  <li key={index} className="ml-5 text-gray-700 list-none flex ">
                    {movie.title}
                    <Button
                      type="button"
                      onClick={() => removeMovie(movie.uniqueId)}
                      className="content-center size-1 m-1"
                    >
                      <X className="size-4" />
                    </Button>
                  </li>
                ))}
              </ul>
            </>
          )
        }}
      </Form>
    </FormDrawer>
  )
}
